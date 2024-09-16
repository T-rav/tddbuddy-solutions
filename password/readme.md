# Password Validator
Implement a common website feature that allows users to reset their password. It is not a simple single class solution. You will need to create an interface and classes that work together to implement the feature.

Create class to store and validate passwords.
All passwords must be salted and hashed.
Do not interact with an Email server.

Create an `AreValidUserCredentials` method that takes in a userName and password. The method salts and hashes the password to check its validity against what is stored. If it matched it returns true, else false.

Create a `SendResetEmail` method that take in an email address. If it matches what is on record for the user send an email with a validation link. The link must include a randomly generated token that will expire 1 hour after being created.

## Example
`AreValidUserCredentials(“userName”, “password”)`

`SendResetEmail(“emailAddress”)`


## Hints
 - Pass in a mocked repository for password validation.
    - Or if you are feeling brave and using C# write an integration test using a real repo that writes to an in-memory DB. Lots of good test packages to choose from.
 - Pass in a mocked email service for sending email.

