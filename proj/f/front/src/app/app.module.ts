import { NgModule, LOCALE_ID } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule, LocationStrategy, PathLocationStrategy, registerLocaleData } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BreadcrumbService } from './core/services/app.breadcrumb.service';
import { MenuService } from './core/services/app.menu.service';
import { AppMainComponent } from './layouts/app.main.component';
import { AppLoginComponent } from './authentication/app.login.component';
import { AppConfigComponent } from './core/components/app.config.component';
import { AppFooterComponent } from './core/components/app.footer.component';
import { AppMenuComponent } from './core/components/app.menu.component';
import { AppMenuitemComponent } from './core/components/app.menuitem.component';
import { AppSearchComponent } from './core/components/app.search.component';
import { AppTopBarComponent } from './core/components/app.topbar.component';
import { LayoutComponent } from './layouts/layout/layout.component';
import { translateFactory, SharedModule } from './modules/shared/shared.module';
import localeRu from '@angular/common/locales/ru';
import { AppAccessdeniedComponent } from './core/components/pages/app.accessdenied.component';
import { AppErrorComponent } from './core/components/pages/app.error.component';
import { AppNotfoundComponent } from './core/components/pages/app.notfound.component';
import { ProfileModule } from './modules/profile/profile.module';
import { PriceListModule } from './modules/price-list/price-list.module';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';

registerLocaleData(localeRu);

@NgModule({
    imports: [
        AppRoutingModule,
        CommonModule,
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: translateFactory,
                deps: [HttpClient],
            },
            defaultLanguage: 'ru'
        }),
        SharedModule.forRoot(),
        ProfileModule,
        PriceListModule
    ],
    declarations: [
        AppComponent,
        AppMainComponent,
        AppMenuComponent,
        AppMenuitemComponent,
        AppConfigComponent,
        AppTopBarComponent,
        AppSearchComponent,
        AppFooterComponent,
        AppLoginComponent,
        LayoutComponent,
        AppAccessdeniedComponent,
        AppErrorComponent,
        AppNotfoundComponent
    ],
    providers: [
        { provide: LocationStrategy, useClass: PathLocationStrategy },
        { provide: LOCALE_ID, useValue: 'ru-RU' },
        MenuService,
        BreadcrumbService
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}