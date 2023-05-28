import { Component, OnInit } from '@angular/core';
import { Episode } from './api/models/episode.model';
import { EpisodeService } from './api/services/episode.service';
import { EpisodeResult } from './api/models/episode-result';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  episodes: Episode[] = [];

  constructor(private episodeService: EpisodeService) { }

  ngOnInit() {
    this.episodeService.getEpisodes().subscribe((response: EpisodeResult) => {
      this.episodes = response.results;
      console.log(this.episodes);
    });
  }
}