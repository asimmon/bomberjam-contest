import './fontawesome'
import React from "react";
import {render} from 'react-dom';
import {Application} from "./react/application";

const hljs = require('highlight.js/lib/core');

function onDocumentReady(callback: () => void) {
  if (document.readyState === 'complete' || document.readyState === 'interactive') {
    window.setTimeout(callback, 1);
  } else {
    document.addEventListener('DOMContentLoaded', callback);
  }
}

function handleHighlighting(): void {
  hljs.registerLanguage('json', require('highlight.js/lib/languages/json'));

  for (const el of document.querySelectorAll<HTMLElement>('pre code.json')) {
    hljs.highlightBlock(el);
  }
}

onDocumentReady(() => {
  // Adding Bootstrap nav toggler behavior without jQuery
  const navbarToggler = document.querySelector<HTMLElement>("button.navbar-toggler");
  if (navbarToggler) {
    navbarToggler.addEventListener("click", () => {
      const targetSelector = navbarToggler.dataset.target;
      if (targetSelector) {
        const navbar = document.querySelector<HTMLElement>(targetSelector);
        if (navbar) {
          if (navbar.classList.contains("show")) {
            navbar.classList.remove("show");
          } else {
            navbar.classList.add("show");
          }
        }
      }
    });
  }

  // Adding Bootstrap custom file input behavior without jQuery
  document.addEventListener('change', evt => {
    const inputFile = evt.target;
    if (inputFile instanceof HTMLInputElement && inputFile.type === 'file' && inputFile.files && inputFile.files.length > 0 && inputFile.classList.contains('custom-file-input')) {
      if (inputFile.parentElement && inputFile.parentElement.classList.contains('custom-file')) {
        const fileInputLabel = inputFile.parentElement.querySelector<HTMLElement>('.custom-file-label');
        if (fileInputLabel) {
          fileInputLabel.textContent = inputFile.files[0].name;
        }
      }

      if (inputFile.classList.contains('bot-source-code')) {
        const errorMessage = 'Bot archive size cannot exceed 25MB';
        let addErrorHandler = () => alert(errorMessage);
        let removeErrorHandler = () => { };

        if (inputFile.parentElement && inputFile.parentElement.classList.contains('custom-file')) {
          const errorSpan = inputFile.parentElement.querySelector<HTMLElement>('.invalid-feedback');
          if (errorSpan) {
            addErrorHandler = () => errorSpan.textContent = errorMessage;
            removeErrorHandler = () => errorSpan.textContent = '';
          }
        }

        for (let file of inputFile.files) {
          if (file.size > 25 * 1024 * 1024) {
            addErrorHandler();
            return;
          }
        }

        removeErrorHandler();
      }
    }

  }, false);

  // Adding confirmation alerts on buttons
  document.addEventListener('click', evt => {
    const el = evt.target as HTMLElement;
    if (el && (el.tagName === 'A' || el.tagName === 'BUTTON') && el.classList.contains('btn-confirm')) {
      if (!window.confirm('Are you sure?')) {
        evt.preventDefault();
      }
    }
  });

  // Bootstrap downdowns
  const openedDropdowns: HTMLElement[] = [];
  document.addEventListener('click', evt => {
    const el = evt.target as HTMLElement;
    if (el && 'toggle' in el.dataset && el.parentElement?.classList.contains('btn-group') && el.nextElementSibling?.classList.contains('dropdown-menu')) {
      const btnCaret = el;
      const btnGroup = el.parentElement;
      const btnDropdown = el.nextElementSibling;

      if (btnGroup.classList.contains('show')) {
        btnGroup.classList.remove('show');
        btnDropdown.classList.remove('show');
        btnCaret.setAttribute('aria-expanded', 'false');
        const idx = openedDropdowns.indexOf(btnCaret);
        if (idx >= 0) openedDropdowns.splice(idx, 1);
      } else {
        btnGroup.classList.add('show');
        btnDropdown.classList.add('show');
        btnCaret.setAttribute('aria-expanded', 'true');
        openedDropdowns.push(btnCaret);
      }

      evt.preventDefault();
    } else {
      for (let openedDropdown of openedDropdowns) {
        const btnGroup = openedDropdown.parentElement;
        const btnDropdown = openedDropdown.nextElementSibling;

        if (btnGroup && btnDropdown) {
          btnGroup.classList.remove('show');
          btnDropdown.classList.remove('show');
          openedDropdown.setAttribute('aria-expanded', 'false');
        }
      }
      openedDropdowns.length = 0;
    }
  });

  // Show current OS download link first
  const currentOs = [...document.documentElement.classList]
    .filter(x => /^os-(windows|linux|macos)$/.test(x))
    .map(x => x.split('-')[1])
    [0];

  if (currentOs) {
    const downloadBtnGroups = document.querySelectorAll<HTMLElement>('.btn-group.downloads');
    for (let i = 0; i < downloadBtnGroups.length; i++) {
      const allBtns = [...downloadBtnGroups[i].querySelectorAll<HTMLAnchorElement>('a.download')];

      if (allBtns.length === 3) {
        const primaryBtn = allBtns.find(x => x.classList.contains('btn'));
        const secondaryBtns = allBtns.filter(x => x !== primaryBtn);

        if (primaryBtn && secondaryBtns.length === 2) {
          const primaryClass = 'download-' + currentOs;
          const sortedBtns = allBtns.sort((a, b) => {
            if (a.classList.contains(primaryClass)) return -1;
            if (b.classList.contains(primaryClass)) return 1;
            return 0;
          });

          const sortedAnchorAttrs = sortedBtns.map(x => ({
            href: x.href,
            innerHTML: x.innerHTML
          }));

          primaryBtn.href = sortedAnchorAttrs[0].href;
          primaryBtn.innerHTML = sortedAnchorAttrs[0].innerHTML;
          secondaryBtns[0].href = sortedAnchorAttrs[1].href;
          secondaryBtns[0].innerHTML = sortedAnchorAttrs[1].innerHTML;
          secondaryBtns[1].href = sortedAnchorAttrs[2].href;
          secondaryBtns[1].innerHTML = sortedAnchorAttrs[2].innerHTML;
        }
      }
    }
  }

  // Render the React game viewer app
  const visualizerEl = document.getElementById('visualizer');
  if (visualizerEl) {
    render(<Application gameId={visualizerEl.dataset['gameId'] ?? ''}/>, visualizerEl);
  }

  // Syntax highlighting
  handleHighlighting();
});