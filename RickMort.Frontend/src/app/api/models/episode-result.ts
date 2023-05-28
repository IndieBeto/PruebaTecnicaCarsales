import { Episode } from "./episode.model";
import { Info } from "./info-model";

export interface EpisodeResult {
    info: Info;
    results: Episode[];
  }