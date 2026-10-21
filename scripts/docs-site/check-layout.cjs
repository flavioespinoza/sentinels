// check-layout.cjs -- load every docs page headless, fail on a 404 or a broken image, report each
// sentinel row's layout (side by side or stacked), and save a screenshot per page.
// Usage: node scripts/docs-site/check-layout.cjs <base url> <screenshot dir> <name suffix> \
//   <path to playwright-core> <path to a chromium binary>
// Serve the site first with scripts/docs-site/serve.sh.
const path = require("path");

const [base, outDir, suffix, playwrightCore, chromiumPath] = process.argv.slice(2);
const { chromium } = require(playwrightCore);
const pages = [
	{ name: "readme", route: "#/README" },
	{ name: "sol-bot", route: "#/_deep-dives/sol-bot-sentinel" },
	{ name: "tribal-knowledge", route: "#/_deep-dives/tribal-knowledge" },
	{ name: "mercy", route: "#/_deep-dives/mercy" },
];

async function checkPage(browser, width, page) {
	const tab = await browser.newPage({ viewport: { width, height: 1000 } });
	const failures = [];
	tab.on("response", (response) => {
		if (response.status() >= 400) failures.push(`${response.status()} ${response.url()}`);
	});
	await tab.goto(`${base}/${page.route}`, { waitUntil: "networkidle" });
	await tab.waitForSelector(".markdown-section .sentinel-row");
	const report = await tab.evaluate(() => {
		const broken = [...document.querySelectorAll(".markdown-section img")]
			.filter((img) => !img.complete || img.naturalWidth === 0)
			.map((img) => img.getAttribute("src"));
		const rows = [...document.querySelectorAll(".sentinel-row")].map((row) => {
			const media = row.querySelector(".sentinel-media").getBoundingClientRect();
			const text = row.querySelector(".sentinel-text").getBoundingClientRect();
			const side = text.left >= media.right && Math.abs(text.top - media.top) < 2;
			return `${side ? "side" : "stacked"} img ${Math.round(media.width)}px`;
		});
		return { broken, rows };
	});
	// The README shot frames the roster, which sits below the fold.
	if (page.name === "readme") {
		await tab.evaluate(() => document.querySelector(".sentinel-row").previousElementSibling.scrollIntoView());
	}
	const file = path.join(outDir, `IMG__sentinels--docs-layout--${width >= 640 ? page.name : page.name === "readme" ? width : `${width}-${page.name}`}--${suffix}.png`);
	await tab.screenshot({ path: file, fullPage: false });
	await tab.close();
	return { width, page: page.name, failures, ...report, file };
}

(async () => {
	const browser = await chromium.launch({ executablePath: chromiumPath });
	let bad = 0;
	for (const width of [1440, 390]) {
		for (const page of pages) {
			const result = await checkPage(browser, width, page);
			bad += result.failures.length + result.broken.length;
			console.log(JSON.stringify(result));
		}
	}
	await browser.close();
	process.exit(bad ? 1 : 0);
})();
