import { Injectable } from '@angular/core';
import { FeatureHub } from 'featurehub-javascript-client-sdk';

@Injectable({ providedIn: 'root' })
export class FeatureService {
  async isLoginEnabled(): Promise<boolean> {
    return FeatureHub.feature('Login').enabled;
  }

  async isRatingEnabled(): Promise<boolean> {
    return FeatureHub.feature('CanRate').enabled;
  }
}
