import 'jquery';
import 'bootstrap';
import { module } from 'angular';

import './fontawesome'

import GameVisualizerController from "./gameVisualizer";

module('bomberjam', [])
  .controller('GameVisualizerController', ['$timeout', GameVisualizerController]);