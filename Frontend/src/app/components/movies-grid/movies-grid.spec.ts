import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MoviesGrid } from './movies-grid';

describe('MoviesGrid', () => {
  let component: MoviesGrid;
  let fixture: ComponentFixture<MoviesGrid>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MoviesGrid]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MoviesGrid);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
