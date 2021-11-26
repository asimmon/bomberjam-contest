import { library, dom } from "@fortawesome/fontawesome-svg-core";
import { faPlay } from '@fortawesome/free-solid-svg-icons/faPlay';
import {  faPause } from '@fortawesome/free-solid-svg-icons/faPause';
import { faVideo } from '@fortawesome/free-solid-svg-icons/faVideo';
import { faExternalLinkAlt } from '@fortawesome/free-solid-svg-icons/faExternalLinkAlt';
import { faDownload } from '@fortawesome/free-solid-svg-icons/faDownload';
import { faBomb } from '@fortawesome/free-solid-svg-icons/faBomb';
import { faWindows } from '@fortawesome/free-brands-svg-icons/faWindows';
import { faLinux } from '@fortawesome/free-brands-svg-icons/faLinux';
import { faApple } from '@fortawesome/free-brands-svg-icons/faApple';

library.add(faPlay, faPause, faVideo, faExternalLinkAlt, faDownload, faBomb, faWindows, faLinux, faApple);
dom.watch();