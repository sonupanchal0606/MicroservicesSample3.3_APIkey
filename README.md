# MicroservicesSample3

---

## 🚀 Run and Test

Start ProductService (5001) and OrderService (5002)
Run Ocelot Gateway (9000)
Make API calls via postman to:
```
GET https://localhost:9000/orders
Header: x-api-key: super-secret-key
```
<img width="1049" alt="image" src="https://github.com/user-attachments/assets/6e5ee435-46af-4154-b756-3024c81a18d4" />

<img width="1051" alt="image" src="https://github.com/user-attachments/assets/d62f55ad-e0ec-4386-a882-e02e8029677d" />

---

| Method                     | Secure | Recommended for                |
| -------------------------- | ------ | ------------------------------ |
| Hardcoded in `ocelot.json` | ❌     | Simple demos or internal tools |
| Environment variables      | ✅      | Dev/prod                       |
| Secret manager (vault)     | ✅✅     | Production apps                |
