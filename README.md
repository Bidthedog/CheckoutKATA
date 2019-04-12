# KATA Checkout Service

Checkout KATA test implemented by [Chris Sebok](https://github.com/Bidthedog) as per the below spec and guidelines.

Granular commits are for demonstration purposes only; commits would normally consist of a full unit test and implementation, including some dependency modifications. I would also normally branch using gitflow for feature updates, and use a mocking framework like Moq instead of hard-coded dependencies.

# Specification

## Checkout Kata

> It is recommended you review the [submission guidelines](#submission-guidelines) before starting the exercise.

Implement the code for a checkout system that handles pricing schemes such as "pineapples cost 50, three pineapples cost 130."

Implement the code for a supermarket checkout that calculates the total price of a number of items. In a normal supermarket, things are identified using Stock Keeping Units, or SKUs. In our store, we’ll use individual letters of the alphabet (A, B, C, and so on). Our goods are priced individually. In addition, some items are multi-priced: buy n of them, and they’ll cost you y pence. For example, item A might cost 50 individually, but this week we have a special offer: buy three As and they’ll cost you 130. In fact the prices are:

| SKU  | Unit Price | Special Price |
| ---- | ---------- | ------------- |
| A    | 50         | 3 for 130     |
| B    | 30         | 2 for 45      |
| C    | 20         |               |
| D    | 15         |               |

The checkout accepts items in any order, so that if we scan a B, an A, and another B, we’ll recognize the two Bs and price them at 45 (for a total price so far of 95). **The pricing changes frequently, so pricing should be independent of the checkout.**

The interface to the checkout could look like:

```cs
interface ICheckout
{
    void Scan(string item);
    int GetTotalPrice();
}
```

## Submission Guidelines

> Please note: This guidance does not replace any specific guidance you have been given during the recruitment process.

So, you've been asked to complete a Kata as part of your application process? What's that all about?!

We use a technical challenge as part of the recruitment process to give you the opportunity to demonstrate your skills and ability. We review the submissions to decide if we will proceed to the next stage of the process - usually a face to face interview. In the face to face interview we review your code with you, either as a discussion exercise, or often as a pairing exercise where we will ask you to extend your code with additional requirements.

When submitting your kata we are specifically looking for:
- Test driven development (TDD)
- Small "baby" steps
- Frequent commits to a repositiory on Github (so we can see _how_ you got to the solution, considering the first 2 points above)
- A README.md if your solution has any specific setup instructions

Don't spend too long on providing your solution - we find a couple of hours will usually suffice.
