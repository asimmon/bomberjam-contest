import 'jquery';
import 'bootstrap';
import { module } from 'angular';

import './fontawesome'

import { Visualizer } from "./gameVisualizer";

module('bomberjam', []).component('visualizer', Visualizer);
