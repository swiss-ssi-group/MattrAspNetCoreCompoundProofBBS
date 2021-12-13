[![.NET](https://github.com/swiss-ssi-group/MattrZeroKnowledgeProofsAspNetCore/actions/workflows/dotnet.yml/badge.svg)](https://github.com/swiss-ssi-group/MattrZeroKnowledgeProofsAspNetCore/actions/workflows/dotnet.yml)

# MATTR ASP.NET Core Compound Proof BBS+ demo

## Blogs

- [Getting started with Self Sovereign Identity SSI](https://damienbod.com/2021/03/29/getting-started-with-self-sovereign-identity-ssi/)
- [Create an OIDC credential Issuer with MATTR and ASP.NET Core](https://damienbod.com/2021/05/03/create-an-oidc-credential-issuer-with-mattr-and-asp-net-core/)
- [Present and Verify Verifiable Credentials in ASP.NET Core using Decentralized Identities and MATTR](https://damienbod.com/2021/05/10/present-and-verify-verifiable-credentials-in-asp-net-core-using-decentralized-identities-and-mattr/)
- [Verify vaccination data using Zero Knowledge Proofs with ASP.NET Core and MATTR](https://damienbod.com/2021/05/31/verify-vaccination-data-using-zero-knowledge-proofs-with-asp-net-core-and-mattr/)
- [Challenges to Self Sovereign Identity](https://damienbod.com/2021/10/11/challenges-to-self-sovereign-identity/)
- [Implement Compound Proof BBS+ verifiable credentials using ASP.NET Core and MATTR](https://damienbod.com/2021/12/13/implement-compound-proof-bbs-verifiable-credentials-using-asp-net-core-and-mattr/https://damienbod.com/2021/12/13/implement-compound-proof-bbs-verifiable-credentials-using-asp-net-core-and-mattr/)

## Test run the applications

## E-ID Verifiable Credentials Issuer (OIDC Credential Issuer)

 - Get an account from MATTR (see MATTR docs)
 - Add the secrets to your configuration
 - Initialize your database
 - Install a MATTR Wallet on your phone
 - start application 

 ## County Residence Verifiable Credentials Issuer (OIDC Credential Issuer)

 - Get an account from MATTR (see MATTR docs)
 - Add the secrets to your configuration
 - Initialize your database
 - Install a MATTR Wallet on your phone
 - start application 

## Verify County Residence and E-ID Compound Verifiable Credentials

 - Install ngrok for the verifier application (npm)
 - Add the secrets to your configuration
 - Initialize your database
 - Start application using for example http://localhost:5000
 - Start ngrok using **ngrok http http://localhost:5000** (like above)
 - Copy the DID for the OIDC Issuer Credentials from the Two Credentials Issuer UIs
 - Create a compound presentation template in the Verify application (Use copied DID)
 - Verify using the wallet and the application

## secrets

```
{
  // use user secrets
  "ConnectionStrings": {
    "DefaultConnection": "--your-connection-string--"
  },
  "MattrConfiguration": {
    "Audience": "https://vii.mattr.global",
    "ClientId": "--your-client-id--",
    "ClientSecret": "--your-client-secret--",
    "TenantId": "--your-tenant--",
    "TenantSubdomain": "--your-tenant-sub-domain--",
    "Url": "http://mattr-prod.au.auth0.com/oauth/token"
  },
  "Auth0": {
    "Domain": "--your-auth0-domain",
    "ClientId": "--your--auth0-client-id--",
    "ClientSecret": "--your-auth0-client-secret--",
  }
  "Auth0Wallet": {
    "Domain": "--your-auth0-wallet-domain",
    "ClientId": "--your--auth0-wallet-client-id--",
    "ClientSecret": "--your-auth0-wallet-client-secret--",
  }
}
```

```
"date_of_birth": "1953-07-21",
"first_name": "Lammy",
"name": "Bob",
"family_name": "Bob",
"given_name": "Lammy",
"birth_place": "Seattle",
"gender": "Male",
"height": "176cm",
"nationality": "USA",

"address_country": "Schweiz",
"address_locality": "Thun",
"address_region": "Bern",
"postal_code": "3000",
"street_address": "Thunerstrasse 14"
                
```

### Auth0 Auth pipeline rules

```
function (user, context, callback) {
    const namespace = 'https://damianbod-sandbox.vii.mattr.global/';
    context.idToken[namespace + 'name'] = user.user_metadata.name;
    context.idToken[namespace + 'first_name'] = user.user_metadata.first_name;
    context.idToken[namespace + 'date_of_birth'] = user.user_metadata.date_of_birth;
  
    context.idToken[namespace + 'family_name'] = user.user_metadata.family_name;
    context.idToken[namespace + 'given_name'] = user.user_metadata.given_name;

    context.idToken[namespace + 'birth_place'] = user.user_metadata.birth_place;
    context.idToken[namespace + 'gender'] = user.user_metadata.gender;
    context.idToken[namespace + 'height'] = user.user_metadata.height;
    context.idToken[namespace + 'nationality'] = user.user_metadata.nationality;
  
    context.idToken[namespace + 'address_country'] = user.user_metadata.address_country;
    context.idToken[namespace + 'address_locality'] = user.user_metadata.address_locality;
    context.idToken[namespace + 'address_region'] = user.user_metadata.address_region;
    context.idToken[namespace + 'street_address'] = user.user_metadata.street_address;
    context.idToken[namespace + 'postal_code'] = user.user_metadata.postal_code;
  
    callback(null, user, context);
}
```

## History

2021-12-04 Initial credentials

## Creating Migrations

### Console

```
dotnet ef migrations add vc_issuer_init
```

### Powershell

```
Add-Migration "vc_issuer_init"
```

## Running Migrations

### Console

```
dotnet restore

dotnet ef database update --context CountyResidenceDataMattrContext

dotnet ef database update --context EidDataMattrContext
```

### Powershell

```
Update-Database 
```


## Links

https://w3c.github.io/json-ld-framing/

https://github.com/admin-ch/CovidCertificate-Apidoc

https://mattr.global/

https://mattr.global/get-started/

https://learn.mattr.global/

https://keybase.io/

https://www.youtube.com/watch?v=2_TDN-81ytM

https://learn.mattr.global/tutorials/dids/did-key

https://gunnarpeipman.com/httpclient-remove-charset/

https://www.lfph.io/wp-content/uploads/2021/02/Verifiable-Credentials-Flavors-Explained.pdf

https://www.xtseminars.co.uk/post/introduction-to-the-future-of-identity-dids-vcs

https://medium.com/decentralized-identity/where-to-begin-with-oidc-and-siop-7dd186c89796

https://www.evernym.com/blog/zero-knowledge-proofs/

https://www.lfph.io/wp-content/uploads/2021/02/Verifiable-Credentials-Flavors-Explained.pdf

https://anonyome.com/2020/06/decentralized-identity-key-concepts-explained/

https://techcommunity.microsoft.com/t5/identity-standards-blog/advancing-privacy-with-zero-knowledge-proof-credentials/ba-p/1441554

# Mattr.Global instructions 

In order to obtain a Credential on the mobile wallet you will need to use the OIDC Bridge, so try following this tutorial.

https://learn.mattr.global/tutorials/issue/oidc-bridge/issue-oidc

At the end of the tutorial you will have a client-bound Credential stored on the mobile wallet.
You can then move to Verify a Credential tutorials, first setup a Presentation Template:

https://learn.mattr.global/tutorials/verify/presentation-request-template

Then you can setup your tenant to run the Verify flow, a quick way of doing that is to use a Sample App to orchestrate a number of steps: 

https://learn.mattr.global/tutorials/verify/using-callback/callback-intro

Note: because you just have the 1 sandbox tenant, you will be issuing credentials and verifying them through the same instance, but Issuer and Verifier could easily be separate tenants on our platform or indeed any other interoperable platform.


## Verifing a credential

https://learn.mattr.global/tutorials/verify/using-callback/callback-local

```
ngrok http http://localhost:5000
```


https://learn.mattr.global/tutorials/verify/using-callback/callback-e-to-e

