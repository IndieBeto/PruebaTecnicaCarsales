import { Component, Input } from '@angular/core';
import { Episode } from 'src/app/api/models/episode.model';

@Component({
  selector: 'app-episode-card',
  templateUrl: './episode-card.component.html',
  styleUrls: ['./episode-card.component.css']
})
export class EpisodeCardComponent {
  @Input() episode!: Episode;
}