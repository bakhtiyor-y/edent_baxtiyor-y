import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { PrimeNGConfig } from 'primeng/api';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
    menuMode = 'static';

    colorScheme = 'light';

    menuTheme = 'layout-sidebar-darkgray';

    inputStyle = 'outlined';

    ripple: boolean;

    constructor(private primengConfig: PrimeNGConfig, private translateService: TranslateService) {
        this.translateService.addLangs(['ru', 'en', 'uz']);
        this.translateService.setDefaultLang('ru');
        this.translateService.use('ru');
    }

    ngOnInit() {
        this.primengConfig.ripple = true;
    }

    translate(lang: string) {
        this.translateService.use(lang);
        this.translateService.get('primeng').subscribe(res => this.primengConfig.setTranslation(res));
    }
}
