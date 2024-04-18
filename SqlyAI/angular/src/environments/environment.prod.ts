import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'SqyAI',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44342/',
    redirectUri: baseUrl,
    clientId: 'SqyAI_App',
    responseType: 'code',
    scope: 'offline_access SqyAI',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44342',
      rootNamespace: 'SqyAI',
    },
  },
} as Environment;
