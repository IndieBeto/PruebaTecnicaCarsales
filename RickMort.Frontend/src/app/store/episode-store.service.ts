import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { EpisodeService } from '../api/services/episode.service';
import { EpisodeResult } from '../api/models/episode-result';


@Injectable({
  providedIn: 'root'
})
export class EpisodeStoreService {
  private episodesSubject: BehaviorSubject<EpisodeResult | null> = new BehaviorSubject<EpisodeResult | null>(null);
  public episodes$: Observable<EpisodeResult | null> = this.episodesSubject.asObservable();

  constructor(private episodeService: EpisodeService) {}

  getEpisodes(page: number = 1): void {
    this.episodeService.getEpisodes(page).subscribe(
      (data: EpisodeResult) => {
        this.episodesSubject.next(data);
      },
      (error) => {
        console.error('Failed to fetch episodes:', error);
      }
    );
  }
}