import { Component, OnInit } from '@angular/core';
import { DatabasesService } from '../services/databases.service';

@Component({
  selector: 'lib-databases',
  template: ` <p>databases works!</p> `,
  styles: [],
})
export class DatabasesComponent implements OnInit {
  constructor(private service: DatabasesService) {}

  ngOnInit(): void {
    this.service.sample().subscribe(console.log);
  }
}
