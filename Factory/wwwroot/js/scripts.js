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

function showModal(type) {
  document.body.classList.add('veiled');
  document.getElementById(`${type}-modal`).classList.remove('obscured');
}

function hideModal(type) {
  document.body.classList.remove('veiled');
  document.getElementById(`${type}-modal`).classList.add('obscured');
}
