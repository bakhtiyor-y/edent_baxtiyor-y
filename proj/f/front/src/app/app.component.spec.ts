import { TestBed, async } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { AppMainComponent } from './layouts/app.main.component';
import { AppConfigComponent } from './core/components/app.config.component';
import { AppTopBarComponent } from './core/components/app.topbar.component';
import { AppMenuComponent } from './core/components/app.menu.component';
import { AppFooterComponent } from './core/components/app.footer.component';

describe('AppComponent', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [NoopAnimationsModule, RouterTestingModule],
            declarations: [
                AppComponent,
                AppMainComponent,
                AppConfigComponent,
                AppTopBarComponent,
                AppMenuComponent,
                AppFooterComponent,
            ]
        }).compileComponents();
    }));
    it('should create the app', async(() => {
        const fixture = TestBed.createComponent(AppComponent);
        const app = fixture.debugElement.componentInstance;
        expect(app).toBeTruthy();
    }));
});
