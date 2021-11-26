import { library, dom } from "@fortawesome/fontawesome-svg-core";
import { faPlay, faPause, faVideo, faExternalLinkAlt, faDownload } from '@fortawesome/free-solid-svg-icons';
import { faWindows, faLinux, faApple } from '@fortawesome/free-brands-svg-icons';

library.add(faPlay, faPause, faVideo, faExternalLinkAlt, faDownload, faWindows, faLinux, faApple);
dom.watch();