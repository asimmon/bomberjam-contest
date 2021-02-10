import './fontawesome'
import React from "react";
import { render } from 'react-dom';
import { onDocumentReady } from './utils';
import { Visualizer } from './visualizer'

onDocumentReady(() => {
  const visualizerEl = document.getElementById('visualizer');
  if (visualizerEl) {
    render(<Visualizer gameId={visualizerEl.dataset['gameId'] ?? ''}/>, visualizerEl);
  }
});