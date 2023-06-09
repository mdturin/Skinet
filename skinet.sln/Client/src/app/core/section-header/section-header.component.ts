import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subject, takeUntil } from 'rxjs';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-section-header',
  templateUrl: './section-header.component.html',
  styleUrls: ['./section-header.component.scss']
})
export class SectionHeaderComponent implements OnInit, OnDestroy {
  notifier = new Subject<void>();
  breadcrumb$: Observable<any[]>;

  constructor(private bcService: BreadcrumbService){}

  ngOnDestroy(): void {
    this.notifier.next();
    this.notifier.complete();
  }

  ngOnInit(): void {
    this.breadcrumb$ = this.bcService.breadcrumbs$;
  }
}
