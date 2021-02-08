import 'jquery';
import 'bootstrap';
import * as angular from 'angular';

import GameVisualizerController from "./gameVisualizer";

angular.module('bomberjam', [])
  .controller('GameVisualizerController', ['$timeout', GameVisualizerController]);