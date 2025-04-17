export const removeMapFlag = () => {
	const link = document.querySelector('a[title="A JavaScript library for interactive maps"]');

	if (link) {
		const svgElement = link.querySelector('svg');

		if (svgElement)
			svgElement.remove();
	}
}