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
      if (inputFile.parentElement && inputFile.parentElement.classList.contains("custom-file")) {
        const fileInputLabel = inputFile.parentElement.querySelector<HTMLElement>(".custom-file-label");
        if (fileInputLabel) {
          fileInputLabel.textContent = inputFile.files[0].name;
        }
      }
    }

  }, false);

  // Render the React game viewer app
  const visualizerEl = document.getElementById('visualizer');
  if (visualizerEl) {
    render(<Application gameId={visualizerEl.dataset['gameId'] ?? ''}/>, visualizerEl);
  }

  // Syntax highlighting
  handleHighlighting();
});