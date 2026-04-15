import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateMoviePage } from './create-movie-page';

describe('CreateMoviePage', () => {
  let component: CreateMoviePage;
  let fixture: ComponentFixture<CreateMoviePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateMoviePage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateMoviePage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
