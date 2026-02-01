# HW2

God Object — це анти-патерн, при якому один клас виконує забагато обовʼязків і фактично централізує логіку системи. Такий клас має низьку звʼязність, сильні залежності, його складно тестувати і змінювати, тому він зазвичай порушує принцип SRP (Single Responsibility Principle), згідно з яким клас повинен мати лише одну причину для змін.
Приклад простого класу, що порушує SRP:

```python
class UserService:
    def register(self, email, password):
        if "@" not in email:
            raise ValueError("Invalid email")
        print("Save user to database")
        print("Send welcome email")
```
У цьому прикладі один клас одночасно відповідає за валідацію даних, збереження користувача та відправку повідомлень, тобто має кілька різних відповідальностей. Для дотримання SRP клас можна відрефакторити, винісши валідацію в окрему функцію, щоб UserService відповідав лише за виконання сценарію реєстрації:

```python
def validate_email(email):
    if "@" not in email:
        raise ValueError("Invalid email")

class UserService:
    def register(self, email, password):
        validate_email(email)
        print("Save user to database")
        print("Send welcome email")
```

Після такого рефакторингу відповідальності розділені, код стає простішим і клас не перетворюється на God Object.