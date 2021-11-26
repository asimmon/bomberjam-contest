import { library, dom } from "@fortawesome/fontawesome-svg-core";
import { faPlay, faPause, faVideo, faExternalLinkAlt, faDownload, faBomb } from '@fortawesome/free-solid-svg-icons';
import { faWindows, faLinux, faApple } from '@fortawesome/free-brands-svg-icons';

library.add(faPlay, faPause, faVideo, faExternalLinkAlt, faDownload, faBomb, faWindows, faLinux, faApple);
dom.watch();