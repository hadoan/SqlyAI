import { ModuleWithProviders, NgModule } from '@angular/core';
import { DATABASES_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class DatabasesConfigModule {
  static forRoot(): ModuleWithProviders<DatabasesConfigModule> {
    return {
      ngModule: DatabasesConfigModule,
      providers: [DATABASES_ROUTE_PROVIDERS],
    };
  }
}
