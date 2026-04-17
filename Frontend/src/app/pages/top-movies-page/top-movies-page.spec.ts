import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopMoviesPage } from './top-movies-page';

describe('TopMoviesPage', () => {
  let component: TopMoviesPage;
  let fixture: ComponentFixture<TopMoviesPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TopMoviesPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TopMoviesPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
