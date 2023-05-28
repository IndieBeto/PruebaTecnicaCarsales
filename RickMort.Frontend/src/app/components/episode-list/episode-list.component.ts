import { Component, OnInit } from '@angular/core';
import { EpisodeResult } from 'src/app/api/models/episode-result';
import { Episode } from 'src/app/api/models/episode.model';
import { EpisodeStoreService } from 'src/app/store/episode-store.service';

enum SortOrder {
  Ascending = 'ascending',
  Descending = 'descending'
}

@Component({
  selector: 'app-episode-list',
  templateUrl: './episode-list.component.html',
  styleUrls: ['./episode-list.component.css']
})
export class EpisodeListComponent implements OnInit {
  episodes: Episode[] = [];
  currentPage: number = 1;
  totalPages: number = 1;
  activeSortButton: string = '';
  sortOrders: Record<string, SortOrder> = {};

  constructor(private episodeStore: EpisodeStoreService) { }

  ngOnInit(): void {
    this.episodeStore.episodes$.subscribe((data: EpisodeResult | null) => {
      if (data) {
        this.episodes = data.results;
        this.totalPages = data.info.pages;
        console.log('Episodes:', data);
      }
    });

    this.loadEpisodes(this.currentPage);
  }

  loadEpisodes(page: number): void {
    this.episodeStore.getEpisodes(page);
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadEpisodes(this.currentPage);
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadEpisodes(this.currentPage);
    }
  }

  sortByName(): void {
    this.updateSortOrder('name');
    this.episodes.sort((a, b) => this.compareValues(a.name, b.name));
  }

  sortByEpisodeId(): void {
    this.updateSortOrder('id');
    this.episodes.sort((a, b) => this.compareValues(a.id, b.id));
  }

  compareValues(a: any, b: any): number {
    if (a === b) {
      return 0;
    }
    if (a > b) {
      return this.sortOrders[this.activeSortButton] === SortOrder.Ascending ? 1 : -1;
    }
    return this.sortOrders[this.activeSortButton] === SortOrder.Ascending ? -1 : 1;
  }

  updateSortOrder(sortButton: string): void {
    if (this.activeSortButton === sortButton) {
      this.sortOrders[sortButton] =
        this.sortOrders[sortButton] === SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
    } else {
      this.activeSortButton = sortButton;
      this.sortOrders[sortButton] = SortOrder.Ascending;
    }
  }
}