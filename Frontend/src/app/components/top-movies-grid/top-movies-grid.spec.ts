import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopMoviesGrid } from './top-movies-grid';

describe('TopMoviesGrid', () => {
  let component: TopMoviesGrid;
  let fixture: ComponentFixture<TopMoviesGrid>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TopMoviesGrid]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TopMoviesGrid);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
