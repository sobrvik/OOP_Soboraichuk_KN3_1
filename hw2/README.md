God Object та SRP
1. Анти-патерн God Object
God Object — це клас, який виконує забагато обовʼязків і централізує логіку всієї системи.
Основні ознаки:
багато різних відповідальностей в одному класі;
великий розмір і складні методи;
сильна залежність від інших модулів;
складно тестувати та підтримувати;
порушує принцип SRP.
2. Приклад класу, що порушує SRP
class UserService:
    def register(self, email, password):
        if "@" not in email:
            raise ValueError("Invalid email")

        password_hash = "hash:" + password
        print("Save user to database")
        print("Send welcome email")
Порушення SRP:
Клас одночасно:
валідовує дані;
хешує пароль;
працює з БД;
відправляє повідомлення.
3. Рефакторинг з дотриманням SRP
class UserValidator:
    def validate(self, email):
        if "@" not in email:
            raise ValueError("Invalid email")


class PasswordHasher:
    def hash(self, password):
        return "hash:" + password


class UserService:
    def __init__(self, validator, hasher):
        self.validator = validator
        self.hasher = hasher

    def register(self, email, password):
        self.validator.validate(email)
        password_hash = self.hasher.hash(password)
        print("Save user to database")
Результат:
Кожен клас має одну відповідальність, код простіший, легше тестується і не перетворюється на God Object.
