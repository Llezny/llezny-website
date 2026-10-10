// Thin browser glue for the island demo. All generation, colouring and animation logic lives in C#
// (Components/IslandGenerator.razor); the browser only lets us put pixels on a canvas.
window.islandMap = {
    // Paints a size x size RGBA buffer onto the canvas.
    blit(canvas, size, rgba) {
        const data = new Uint8ClampedArray(rgba.buffer, rgba.byteOffset, rgba.length);
        canvas.getContext('2d').putImageData(new ImageData(data, size, size), 0, 0);
    },

    nextFrame() {
        return new Promise((resolve) => requestAnimationFrame(() => resolve()));
    },

    prefersReducedMotion() {
        return !!(window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches);
    },

    // Calls back into .NET once the canvas is on screen.
    whenVisible(canvas, dotNetRef) {
        if (!('IntersectionObserver' in window)) {
            dotNetRef.invokeMethodAsync('OnVisible');
            return;
        }
        const observer = new IntersectionObserver((entries) => {
            if (entries.some((e) => e.isIntersecting)) {
                observer.disconnect();
                dotNetRef.invokeMethodAsync('OnVisible');
            }
        }, { threshold: 0.3 });
        observer.observe(canvas);
    },
};
