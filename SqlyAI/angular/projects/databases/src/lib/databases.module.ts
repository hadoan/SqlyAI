import { NgModule, NgModuleFactory, ModuleWithProviders } from '@angular/core';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { DatabasesComponent } from './components/databases.component';
import { DatabasesRoutingModule } from './databases-routing.module';

@NgModule({
  declarations: [DatabasesComponent],
  imports: [CoreModule, ThemeSharedModule, DatabasesRoutingModule],
  exports: [DatabasesComponent],
})
export class DatabasesModule {
  static forChild(): ModuleWithProviders<DatabasesModule> {
    return {
      ngModule: DatabasesModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<DatabasesModule> {
    return new LazyModuleFactory(DatabasesModule.forChild());
  }
}
