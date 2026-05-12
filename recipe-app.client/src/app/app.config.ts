import { provideHttpClient, withInterceptors } from "@angular/common/http";
import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from "@angular/core";
import { provideRouter } from "@angular/router";

import { AuthInterceptor } from "../app/core/services/AuthInterceptor";
import { routes } from "./app-routing-module";

export const appConfig: ApplicationConfig = {
    providers: [
        provideBrowserGlobalErrorListeners(),
        provideZonelessChangeDetection(),
        provideHttpClient(withInterceptors([AuthInterceptor])),
        provideRouter(routes)
    ]
};
