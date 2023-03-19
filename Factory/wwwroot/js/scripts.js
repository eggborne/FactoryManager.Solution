const pause = async (ms) => new Promise(resolve => setTimeout(resolve, ms));

const swapClass = (element, oldClass, newClass) => {
  if (element.classList.contains(oldClass)) {
    element.classList.remove(oldClass);
  }
  element.classList.add(newClass);
};

window.addEventListener('load', async () => {
  document.documentElement.style.setProperty('--actual-height', window.innerHeight + 'px');
  document.querySelector('main').classList.remove('obscured');
});
