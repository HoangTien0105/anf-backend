# Table of Contents

- [Acknowledgement](#acknowledgement)
- [Project Information](#project-information)
- [Team Members](#team-members)
- [Product Background](#product-background)
- [Business Opportunity](#business-opportunity)
- [Software Product Vision](#software-product-vision)
- [Project Scope & Limitations](#project-scope--limitations)
  - [Advertiser Features](#advertiser-features)
  - [Publisher Features](#publisher-features)
  - [Administrator Features](#administrator-features)
  - [Shared Features](#shared-features)
- [Context Diagram](#context-diagram)
- [Use Cases](#use-cases)
- [Database Design](#database-design)

---

# Acknowledgement

Throughout the journey, amidst numerous difficulties, conflicts, and arguments, our team has successfully completed the project. Beyond the final outcome, we have gained invaluable lessons in collaboration, perseverance, problem-solving, and mutual respect. This journey has not only strengthened our technical and professional skills but also fostered personal growth and a deeper appreciation for teamwork and resilience.

First and foremost, we extend our deepest and most sincere gratitude to Mr. Nguyen Nguyen Binh, our esteemed supervisor. His profound expertise, unwavering support, and dedicated guidance have been instrumental to the successful completion of our project. His encouragement and insightful advice have consistently inspired and motivated us to strive for excellence.

We also wish to express our heartfelt thanks to the reviewers for their invaluable critiques and constructive feedback. Their contributions have helped us to recognize areas for improvement and refine our work to achieve the highest possible standard.

Our sincere appreciation is extended to FPT University for providing us with an outstanding learning environment and the opportunity to grow both academically and personally through this project.

We are profoundly grateful to our families, whose unwavering support, encouragement, and understanding have been crucial to our well-being and perseverance throughout this journey.

Moreover, we would like to acknowledge and thank all individuals who contributed to the realization of our graduation project.

Finally, we would like to recognize the efforts of our own team. The resilience, collaboration, and dedication we demonstrated throughout every challenge are a source of pride. We are grateful to one another for the commitment and unity that brought this project to fruition.

With deepest respect and gratitude,  
Affiliate Network Team

---

# Project Information

- **Project name:** Affiliate Network - Connecting companies and partners through smart affiliate marketing  
- **Project code:** SP25SP116  
- **Group name:** GSP25SE10  
- **Software type:** Web Application  

---

# Team Members

| Full Name            | Role    | Email                          |
|----------------------|---------|--------------------------------|
| Vũ Long              | Leader  | LongVSE171024@fpt.edu.vn       |
| Văn Hoàng Tiến       | Member  | TienVHSE172309@fpt.edu.vn      |
| Vũ Minh Nhật         | Member  | NhatVMSE172011@fpt.edu.vn      |
| Đặng Kim Minh Tiến   | Member  | TienDKMSE172051@fpt.edu.vn     |
| Trần Minh Toàn       | Member  | ToanTMSE170274@fpt.edu.vn      |

---

# Product Background

Affiliate marketing is a cornerstone of digital advertising, enabling advertisers to boost brand visibility and sales while giving affiliates opportunities to monetize their channels. However, despite its growing adoption, the affiliate marketing ecosystem still grapples with major challenges that limit its full potential. These include fraudulent traffic, a lack of transparency, and inefficiencies in tracking campaign performance—issues that negatively impact both advertisers and affiliates.

The Affiliate Network aims to address these problems by delivering a secure, efficient, and transparent solution for affiliate marketing. The network will provide advertisers with a centralized system to manage their campaigns and monitor performance, while allowing affiliates to earn revenue by promoting offers that resonate with their audiences. Additionally, built-in fraud detection tools and comprehensive analytics will support a fair and optimized environment for all participants.

---

# Business Opportunity

Vietnam's digital market is growing rapidly, with businesses and content creators increasingly turning to affiliate marketing. However, existing platforms often fall short—advertisers face issues with fraud, poor transparency, and scattered campaign management, while affiliates struggle to find reliable, well-matched offers and fair compensation.

Affiliate Network aims to solve these problems by providing a secure, centralized network tailored to Vietnam’s market. By streamlining campaign tracking, ensuring transparency, and integrating strong fraud detection, the project creates a win-win environment where advertisers can scale effectively and affiliates can earn with confidence.

---

# Software Product Vision

Affiliate Network envisions a trusted and transparent digital ecosystem that empowers advertisers and affiliates in Vietnam to collaborate efficiently and profitably. The network will serve as a centralized platform where advertisers can seamlessly manage and optimize campaigns, while affiliates can access high-quality offers aligned with their audiences.

By integrating advanced fraud detection, real-time performance analytics, and intuitive campaign tools, the software will eliminate key inefficiencies found in traditional affiliate systems. The ultimate goal is to foster sustainable growth for all stakeholders through a secure, data-driven, and performance-oriented network.

---

# Project Scope & Limitations

## Advertiser Features

- FE-01: Create a new advertising campaign  
- FE-02: View all campaigns created by the advertiser  
- FE-03: Update campaign details and settings  
- FE-04: Add offers to an existing campaign  
- FE-05: Edit detailed information of existing offers in a campaign  
- FE-06: View information about publishers’ applications in a campaign  
- FE-07: Check publisher’s registration application  
- FE-08: View campaign performance progress  
- FE-09: Register account to become an advertiser  
- FE-10: Edit account information  
- FE-11: Send postback URL to the platform  
- FE-12: Deposit money into an internal wallet using payOS  
- FE-13: Create a request to withdraw the money  
- FE-14: Create complaint ticket  
- FE-15: Support API about campaign creation to integrate other systems  

## Publisher Features

- FE-16: Register as publisher  
- FE-17: Edit profile  
- FE-18: View profile  
- FE-19: Add traffic sources  
- FE-20: Edit traffic sources  
- FE-21: View approved traffic sources  
- FE-22: View available campaigns with offers’ detailed  information  
- FE-23: Send a request to register a campaign  
- FE-24: View performance statistics of a campaign  
- FE-25: View balance in the wallet  
- FE-26: Receive earning report after running campaign successfully  
- FE-27: Create a request to withdraw the money  
- FE-28: Create complaint ticket  

## Administrator Features

- **Category Management**: Add, edit, view, delete subscription  
- **Campaign Management**: Review, approve/reject campaigns, update campaign status  
- **Traffic Source Management**: Approve/reject publisher affiliate sources  
- **User Management**: Approve/reject/remove advertiser or publisher accounts  
- **Internal Wallet Management**: View transactions, approve withdrawals, export payments  
- **Policy Management**: Add, edit, view, delete policy  
- **Subscription Management**: Add, edit, view, delete subscription  

## Shared Features

- FE-36: Login into the system  
- FE-37: Logout  
- FE-38: Real-time notification for campaign/account events  
- FE-39: Reset password for publishers and advertisers  
- FE-40: Image storing with Cloudinary  

---

# Context Diagram

![Affiliate Network Context Diagram](https://drive.google.com/uc?export=view&id=1nqrwFIJrQYZ7GlMxwpdogWh_yPhxQo2U)

## Actors

1. **Advertiser** – Manage campaigns, handle publisher registrations, view performance, etc.  
2. **Publisher** – Register, manage traffic, apply to campaigns, track earnings  
3. **Admin** – Platform moderation and management  
4. **User** – Clicks on links to access advertiser’s site  
5. **payOS** – External payment gateway used for deposits

---

# Use Cases

![Affiliate Network Use Case](https://drive.google.com/uc?export=view&id=1QgjYON0dwDPeIqIviU9kfFHc2ZVxp1wI)
---

# Database Design

- **Conceptual**: ![Affiliate Network Conceptual](https://drive.google.com/uc?export=view&id=10w-S0qwex5K6m-T9Zg5HkScd1xqm8oit)
- **Physical**: ![Affiliate Network Physical](https://drive.google.com/uc?export=view&id=1egAuw-rnkPiAeIHfFfrwYic6X4TIFEJy)
