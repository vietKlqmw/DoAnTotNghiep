import { AbpSessionService } from 'abp-ng2-module';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { SessionServiceProxy, UpdateUserSignInTokenOutput } from '@shared/service-proxies/service-proxies';
import { UrlHelper } from 'shared/helpers/UrlHelper';
import { ExternalLoginProvider, LoginService } from './login.service';
import { ReCaptchaV3Service } from 'ngx-captcha';
import { AppConsts } from '@shared/AppConsts';

@Component({
    templateUrl: './login.component.html',
    animations: [accountModuleAnimation()],
    styleUrls: ['./login.component.less']
})
export class LoginComponent extends AppComponentBase implements OnInit {
    submitting = false;
    isMultiTenancyEnabled: boolean = this.multiTenancy.isEnabled;
    recaptchaSiteKey: string = AppConsts.recaptchaSiteKey;
    captchaResponse?: string;

    constructor(
        injector: Injector,
        public loginService: LoginService,
        private _router: Router,
        private _sessionService: AbpSessionService,
        private _sessionAppService: SessionServiceProxy,
        private _reCaptchaV3Service: ReCaptchaV3Service
    ) {
        super(injector);
    }

    get multiTenancySideIsTeanant(): boolean {
        return this._sessionService.tenantId > 0;
    }

    get isTenantSelfRegistrationAllowed(): boolean {
        return this.setting.getBoolean('App.TenantManagement.AllowSelfRegistration');
    }

    get isSelfRegistrationAllowed(): boolean {
        if (!this._sessionService.tenantId) {
            return false;
        }

        return this.setting.getBoolean('App.UserManagement.AllowSelfRegistration');
    }

    ngOnInit(): void {
        if (this._sessionService.userId > 0 && UrlHelper.getReturnUrl() && UrlHelper.getSingleSignIn()) {
            this._sessionAppService.updateUserSignInToken()
                .subscribe((result: UpdateUserSignInTokenOutput) => {
                    const initialReturnUrl = UrlHelper.getReturnUrl();
                    const returnUrl = initialReturnUrl + (initialReturnUrl.indexOf('?') >= 0 ? '&' : '?') +
                        'accessToken=' + result.signInToken +
                        '&userId=' + result.encodedUserId +
                        '&tenantId=' + result.encodedTenantId;

                    location.href = returnUrl;
                });
        }

        let state = UrlHelper.getQueryParametersUsingHash().state;
        if (state && state.indexOf('openIdConnect') >= 0) {
            this.loginService.openIdConnectLoginCallback({});
        }
    }

    login(): void {
        let recaptchaCallback = () => {
            this.showMainSpinner();

            this.submitting = true;
            this.loginService.authenticate(
                () => {
                    this.submitting = false;
                    this.hideMainSpinner();
                },
                null,
                this.captchaResponse
            );
        };

        if (this.useCaptcha) {
            this._reCaptchaV3Service.execute(this.recaptchaSiteKey, 'login', (token) => {
                this.captchaResponse = token;
                recaptchaCallback();
            });
        } else {
            recaptchaCallback();
        }
    }

    externalLogin(provider: ExternalLoginProvider) {
        this.loginService.externalAuthenticate(provider);
    }

    get useCaptcha(): boolean {
        return this.setting.getBoolean('App.UserManagement.UseCaptchaOnLogin');
    }

    password_eye_click(type:number) {

        let _eyeOpen = document.querySelector<HTMLElement>(".login-account .login_content .login-form .form-group img.password-eye.password-eye-open");
        let _eyeOff = document.querySelector<HTMLElement>(".login-account .login_content .login-form .form-group img.password-eye.password-eye-off");
        let _pass = document.querySelector<HTMLElement>(".login-account .login_content .login-form .form-group input[name=password].input-login");

        if(type == 1) {
            _eyeOff.style.display = "none";
            _eyeOpen.style.display = "block";
            _pass.setAttribute("type","text");
        }
        else {
            _eyeOff.style.display = "block";
            _eyeOpen.style.display = "none";
            _pass.setAttribute("type","password");
        }

    }
}
