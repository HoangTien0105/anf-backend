-- Script chạy data cho affiliate_network_dev và affiliate-network-prod
INSERT INTO affiliate_network_dev.dbo.TrackingParams (name,description) VALUES
	 (N'click_id',N'Unique identifier for each click'),
	 (N'utm_source',N'UTM parameter for traffic source'),
	 (N'utm_campaign',N'UTM parameter for campaign name'),
	 (N'utm_content',N'UTM parameter for content identifier'),
	 (N'utm_term',N'UTM parameter for search terms'),
	 (N'payout',N'Commission amount for this affiliate')
	 (N'source',N'Traffic source identifier'),
	 (N'publisher_id',N'Unique identifier for the affiliate'),
	 (N'campaign_id',N'Specific campaign identifier'),
	 (N'sub_id',N'Sub-affiliate identifier'),
	 (N'channel',N'Marketing channel (email, social, search, etc.)'),
	 (N'keyword',N'Keyword that triggered the ad'),
	 (N'device',N'User device type (desktop, mobile, tablet)'),
	 (N'country',N'User country code'),
	 (N'referrer',N'URL of the referring website'),
	 (N'landing_page',N'Specific landing page URL or identifier');

-- Users
INSERT INTO affiliate_network_dev.dbo.Users (
    user_id, user_code, first_name, last_name, phone_number, citizen_id, address, date_of_birth, 
    user_email, user_password, email_confirmed, user_status, user_role, 
    reset_password_token, token_expired_date, reject_reason, concurrency_stamp
) VALUES
-- Admins
(1, 'ADM1001', NULL, NULL, NULL, NULL, NULL, NULL, 'admin1@example.com', 'hashed_password_admin', 1, 1, 0, NULL, NULL, NULL, DEFAULT),
(2, 'ADM1002', NULL, NULL, NULL, NULL, NULL, NULL, 'admin2@example.com', 'hashed_password_admin', 1, 1, 0, NULL, NULL, NULL, DEFAULT),
(3, 'ADM1003', NULL, NULL, NULL, NULL, NULL, NULL, 'admin3@example.com', 'hashed_password_admin', 1, 1, 0, NULL, NULL, NULL, DEFAULT),
-- Advertisers
(4, 'ADVX1234', 'Nguyen', 'Thanh Lam', '0912345678', '111111111111', 'Ha Noi', '1987-01-22', 'lam.nguyen@example.com', 'hashed_password_adv', 1, 1, 1, NULL, NULL, NULL, DEFAULT),
(5, 'ADVY5678', 'Tran', 'Minh Quang', '0923456789', '222222222222', 'Hai Phong', '1990-02-15', 'quang.tran@example.com', 'hashed_password_adv', 1, 1, 1, NULL, NULL, NULL, DEFAULT),
(6, 'ADVZ9876', 'Pham', 'Thu Ha', '0934567890', '333333333333', 'TP HCM', '1992-05-30', 'ha.pham@example.com', 'hashed_password_adv', 1, 1, 1, NULL, NULL, NULL, DEFAULT),
(7, 'ADVW4321', 'Le', 'Hoai An', '0945678901', '444444444444', 'Da Nang', '1989-08-10', 'an.le@example.com', 'hashed_password_adv', 1, 1, 1, NULL, NULL, NULL, DEFAULT),
(8, 'ADVR7654', 'Do', 'Khanh Nam', '0956789012', '555555555555', 'Can Tho', '1991-09-12', 'nam.do@example.com', 'hashed_password_adv', 1, 1, 1, NULL, NULL, NULL, DEFAULT),
(9, 'ADVQ2134', 'Bui', 'Duc Tien', '0967890123', '666666666666', 'Binh Duong', '1988-11-05', 'tien.bui@example.com', 'hashed_password_adv', 1, 1, 1, NULL, NULL, NULL, DEFAULT),
(10, 'ADVP6543', 'Vu', 'Thanh Hoa', '0978901234', '777777777777', 'Quang Ninh', '1994-06-25', 'hoa.vu@example.com', 'hashed_password_adv', 1, 1, 1, NULL, NULL, NULL, DEFAULT),
(11, 'ADVO8765', 'Ho', 'Minh Hieu', '0989012345', '888888888888', 'Nha Trang', '1993-07-18', 'hieu.ho@example.com', 'hashed_password_adv', 1, 1, 1, NULL, NULL, NULL, DEFAULT),
(12, 'ADVN0987', 'Ly', 'Thuy Duong', '0990123456', '999999999999', 'Hue', '1995-03-08', 'duong.ly@example.com', 'hashed_password_adv', 1, 1, 1, NULL, NULL, NULL, DEFAULT),
(13, 'ADVM5672', 'Dang', 'Bao Khang', '0901234567', '101010101010', 'Vung Tau', '1986-12-01', 'khang.dang@example.com', 'hashed_password_adv', 1, 1, 1, NULL, NULL, NULL, DEFAULT),
-- Publishers
(14, 'PUBA4321', 'Nguyen', 'Van Dung', '0898765432', '111222333444', 'Bac Ninh', '1996-10-17', 'dung.nguyen@example.com', 'hashed_password_pub', 1, 1, 2, NULL, NULL, NULL, DEFAULT),
(15, 'PUBB6543', 'Tran', 'Minh Son', '0887654321', '222333444555', 'Long An', '1987-04-14', 'son.tran@example.com', 'hashed_password_pub', 1, 1, 2, NULL, NULL, NULL, DEFAULT),
(16, 'PUBC9876', 'Pham', 'Quoc Tuan', '0876543210', '333444555666', 'Dong Thap', '1992-02-23', 'tuan.pham@example.com', 'hashed_password_pub', 1, 1, 2, NULL, NULL, NULL, DEFAULT),
(17, 'PUBD1234', 'Le', 'Hong Phuc', '0865432109', '444555666777', 'Binh Phuoc', '1995-06-30', 'phuc.le@example.com', 'hashed_password_pub', 1, 1, 2, NULL, NULL, NULL, DEFAULT),
(18, 'PUBE5678', 'Do', 'Thu Ha', '0854321098', '555666777888', 'Phu Tho', '1990-09-25', 'ha.do@example.com', 'hashed_password_pub', 1, 1, 2, NULL, NULL, NULL, DEFAULT),
(19, 'PUBF7890', 'Bui', 'Thanh Tam', '0843210987', '666777888999', 'Binh Dinh', '1988-11-19', 'tam.bui@example.com', 'hashed_password_pub', 1, 1, 2, NULL, NULL, NULL, DEFAULT),
(20, 'PUBG3210', 'Vu', 'Tuan Minh', '0832109876', '777888999000', 'Quang Tri', '1997-01-11', 'minh.vu@example.com', 'hashed_password_pub', 1, 1, 2, NULL, NULL, NULL, DEFAULT),
(21, 'PUBH4321', 'Ho', 'Duy Anh', '0821098765', '888999000111', 'Tay Ninh', '1994-03-05', 'anh.ho@example.com', 'hashed_password_pub', 1, 1, 2, NULL, NULL, NULL, DEFAULT),
(22, 'PUBI5432', 'Ly', 'Van Tinh', '0810987654', '999000111222', 'Lao Cai', '1989-07-08', 'tinh.ly@example.com', 'hashed_password_pub', 1, 1, 2, NULL, NULL, NULL, DEFAULT),
(23, 'PUBJ6543', 'Dang', 'Minh Chau', '0809876543', '000111222333', 'Ha Giang', '1993-05-27', 'chau.dang@example.com', 'hashed_password_pub', 1, 1, 2, NULL, NULL, NULL, DEFAULT),
(24, 'PUBK7654', 'Trinh', 'Thi Hanh', '0798765432', '111222333444', 'Kon Tum', '1991-08-15', 'hanh.trinh@example.com', 'hashed_password_pub', 1, 1, 2, NULL, NULL, NULL, DEFAULT),
(25, 'PUBL8765', 'Mai', 'Hoang Linh', '0787654321', '222333444555', 'Son La', '1986-12-10', 'linh.mai@example.com', 'hashed_password_pub', 1, 1, 2, NULL, NULL, NULL, DEFAULT);

INSERT INTO affiliate_network_dev.dbo.PublisherProfiles (
    specialization, image_url, bio, publisher_id
) VALUES
-- Profiles for Publishers
('Digital Marketing', 'https://t4.ftcdn.net/jpg/02/29/75/83/360_F_229758328_7x8jwCwjtBMmC6rgFzLFhZoEpLobB6L8.jpg', 'Experienced in SEO, PPC, and content marketing for affiliate campaigns.', 14),
('E-commerce Affiliate', 'https://t4.ftcdn.net/jpg/02/29/75/83/360_F_229758328_7x8jwCwjtBMmC6rgFzLFhZoEpLobB6L8.jpg', 'Specializes in driving traffic and conversions for online stores.', 15),
('Social Media Marketing', 'https://t4.ftcdn.net/jpg/02/29/75/83/360_F_229758328_7x8jwCwjtBMmC6rgFzLFhZoEpLobB6L8.jpg', 'Expert in leveraging social platforms for brand promotions and affiliate growth.', 16),
('Email Marketing', 'https://t4.ftcdn.net/jpg/02/29/75/83/360_F_229758328_7x8jwCwjtBMmC6rgFzLFhZoEpLobB6L8.jpg', 'Builds high-converting email funnels for affiliate programs.', 17),
('Influencer Marketing', 'https://t4.ftcdn.net/jpg/02/29/75/83/360_F_229758328_7x8jwCwjtBMmC6rgFzLFhZoEpLobB6L8.jpg', 'Collaborates with influencers to promote affiliate products effectively.', 18),
('Lead Generation', 'https://t4.ftcdn.net/jpg/02/29/75/83/360_F_229758328_7x8jwCwjtBMmC6rgFzLFhZoEpLobB6L8.jpg', 'Focuses on B2B and B2C lead generation strategies for affiliate networks.', 19),
('Content Marketing', 'https://t4.ftcdn.net/jpg/02/29/75/83/360_F_229758328_7x8jwCwjtBMmC6rgFzLFhZoEpLobB6L8.jpg', 'Creates high-quality content to drive organic traffic and affiliate sales.', 20),
('Performance Marketing', 'https://t4.ftcdn.net/jpg/02/29/75/83/360_F_229758328_7x8jwCwjtBMmC6rgFzLFhZoEpLobB6L8.jpg', 'Uses data-driven methods to optimize affiliate campaign ROI.', 21),
('SEO Specialist', 'https://t4.ftcdn.net/jpg/02/29/75/83/360_F_229758328_7x8jwCwjtBMmC6rgFzLFhZoEpLobB6L8.jpg', 'Helps brands rank higher on search engines for affiliate success.', 22),
('Affiliate Program Manager', 'https://t4.ftcdn.net/jpg/02/29/75/83/360_F_229758328_7x8jwCwjtBMmC6rgFzLFhZoEpLobB6L8.jpg', 'Manages partnerships and networks to maximize affiliate revenue.', 23),
('Conversion Optimization', 'https://t4.ftcdn.net/jpg/02/29/75/83/360_F_229758328_7x8jwCwjtBMmC6rgFzLFhZoEpLobB6L8.jpg', 'Expert in improving landing pages and user journeys for higher conversions.', 24),
('Sales Funnel Expert', 'https://t4.ftcdn.net/jpg/02/29/75/83/360_F_229758328_7x8jwCwjtBMmC6rgFzLFhZoEpLobB6L8.jpg', 'Builds and optimizes multi-step sales funnels for affiliate offers.', 25);

INSERT INTO affiliate_network_dev.dbo.AdvertiserProfiles (
    company_name, industry, image_url, bio, advertiser_id
) VALUES
-- Profiles for Advertisers
('Tech Innovations Ltd.', 'Technology', 'https://cdnphoto.dantri.com.vn/0nFCO06Gmn3AEsryYNxc-0zzCmI=/thumb_w/960/2020/01/15/nash-tech-primary-poss-rgb-1579062141797.png', 'Leading provider of innovative software solutions and SaaS platforms.', 2),
('GreenMarket Solutions', 'E-commerce', 'https://cdnphoto.dantri.com.vn/0nFCO06Gmn3AEsryYNxc-0zzCmI=/thumb_w/960/2020/01/15/nash-tech-primary-poss-rgb-1579062141797.png', 'Sustainable e-commerce platform specializing in eco-friendly products.', 3),
('HealthPlus Supplements', 'Health & Wellness', 'https://cdnphoto.dantri.com.vn/0nFCO06Gmn3AEsryYNxc-0zzCmI=/thumb_w/960/2020/01/15/nash-tech-primary-poss-rgb-1579062141797.png', 'Manufacturer of organic supplements and wellness products.', 4),
('TravelGo Agency', 'Travel & Hospitality', 'https://cdnphoto.dantri.com.vn/0nFCO06Gmn3AEsryYNxc-0zzCmI=/thumb_w/960/2020/01/15/nash-tech-primary-poss-rgb-1579062141797.png', 'Global travel agency offering customized vacation packages.', 5),
('SmartGadgets Inc.', 'Consumer Electronics', 'https://cdnphoto.dantri.com.vn/0nFCO06Gmn3AEsryYNxc-0zzCmI=/thumb_w/960/2020/01/15/nash-tech-primary-poss-rgb-1579062141797.png', 'Retailer of innovative gadgets and smart home devices.', 6),
('EduGrowth Academy', 'Education', 'https://cdnphoto.dantri.com.vn/0nFCO06Gmn3AEsryYNxc-0zzCmI=/thumb_w/960/2020/01/15/nash-tech-primary-poss-rgb-1579062141797.png', 'Online learning platform providing courses in various domains.', 7),
('FitnessPro Gear', 'Fitness & Sports', 'https://cdnphoto.dantri.com.vn/0nFCO06Gmn3AEsryYNxc-0zzCmI=/thumb_w/960/2020/01/15/nash-tech-primary-poss-rgb-1579062141797.png', 'E-commerce store specializing in fitness equipment and apparel.', 8),
('HomeDecor Trends', 'Home & Living', 'https://cdnphoto.dantri.com.vn/0nFCO06Gmn3AEsryYNxc-0zzCmI=/thumb_w/960/2020/01/15/nash-tech-primary-poss-rgb-1579062141797.png', 'Curated collection of stylish and modern home décor solutions.', 9),
('AutoParts World', 'Automotive', 'https://cdnphoto.dantri.com.vn/0nFCO06Gmn3AEsryYNxc-0zzCmI=/thumb_w/960/2020/01/15/nash-tech-primary-poss-rgb-1579062141797.png', 'Marketplace for high-quality car parts and accessories.', 10),
('FashionForward Co.', 'Fashion & Apparel', 'https://cdnphoto.dantri.com.vn/0nFCO06Gmn3AEsryYNxc-0zzCmI=/thumb_w/960/2020/01/15/nash-tech-primary-poss-rgb-1579062141797.png', 'Trendy fashion retailer offering the latest styles and accessories.', 11);

INSERT INTO affiliate_network_dev.dbo.TrafficSources (
    publisher_id, provider, soruce_url, created_at, [type], status
) VALUES
-- Traffic sources for Publishers
(14, 'Facebook', 'https://facebook.com/publisher14', GETDATE(), 'Social Media', 1),
(15, 'Instagram', 'https://instagram.com/publisher15', GETDATE(), 'Social Media', 1),
(16, 'TikTok', 'https://tiktok.com/@publisher16', GETDATE(), 'Social Media', 1),
(17, 'Google Ads', 'https://ads.google.com/publisher17', GETDATE(), 'Paid Advertising', 1),
(18, 'YouTube', 'https://youtube.com/channel/publisher18', GETDATE(), 'Video Marketing', 1),
(19, 'Twitter', 'https://twitter.com/publisher19', GETDATE(), 'Social Media', 1),
(20, 'LinkedIn', 'https://linkedin.com/in/publisher20', GETDATE(), 'Professional Networking', 1),
(21, 'Pinterest', 'https://pinterest.com/publisher21', GETDATE(), 'Visual Discovery', 1),
(22, 'Reddit', 'https://reddit.com/u/publisher22', GETDATE(), 'Community Marketing', 1),
(23, 'Quora', 'https://quora.com/profile/publisher23', GETDATE(), 'Content Marketing', 1),
(24, 'Snapchat', 'https://snapchat.com/add/publisher24', GETDATE(), 'Social Media', 1),
(25, 'Medium', 'https://medium.com/@publisher25', GETDATE(), 'Blogging', 1);

INSERT INTO affiliate_network_dev.dbo.UserBank (
    user_code, banking_no, banking_provider, added_date, user_name
) VALUES
-- Bank accounts for Advertisers
('ADV002', 9704234567890123, 'VIETCOMBANK', GETDATE(), 'TRAN THANH BINH'),
('ADV003', 9704567890123456, 'TECHCOMBANK', GETDATE(), 'LE HOANG NAM'),
('ADV004', 9705678901234567, 'BIDV', GETDATE(), 'PHAM MINH CHAU'),
('ADV005', 9706789012345678, 'VIETINBANK', GETDATE(), 'VO TUAN KIET'),
('ADV006', 9707890123456789, 'AGRIBANK', GETDATE(), 'NGUYEN PHUONG LINH'),
('ADV007', 9708901234567890, 'ACB', GETDATE(), 'DO VAN HOANG'),
('ADV008', 9709012345678901, 'SACOMBANK', GETDATE(), 'BUI THI HONG'),
('ADV009', 9700123456789012, 'MB BANK', GETDATE(), 'HO ANH TUAN'),
('ADV010', 9701234567890123, 'VPBANK', GETDATE(), 'LY VAN HIEU'),
('ADV011', 9702345678901234, 'SHB', GETDATE(), 'NGUYEN THI MAI'),
-- Bank accounts for Publishers
('PUB009', 9703456789012345, 'VIETCOMBANK', GETDATE(), 'HO ANH TUAN'),
('PUB010', 9704567890123456, 'TECHCOMBANK', GETDATE(), 'LY VAN HIEU'),
('PUB011', 9705678901234567, 'BIDV', GETDATE(), 'NGUYEN THI MAI'),
('PUB012', 9706789012345678, 'VIETINBANK', GETDATE(), 'PHAM DUC THINH'),
('PUB013', 9707890123456789, 'AGRIBANK', GETDATE(), 'VO NGOC LAN'),
('PUB014', 9708901234567890, 'ACB', GETDATE(), 'TRAN THANH DAT'),
('PUB015', 9709012345678901, 'SACOMBANK', GETDATE(), 'DANG MINH KHOA'),
('PUB016', 9700123456789012, 'MB BANK', GETDATE(), 'TRAN THANH BINH'),
('PUB017', 9701234567890123, 'VPBANK', GETDATE(), 'LE HOANG NAM'),
('PUB018', 9702345678901234, 'SHB', GETDATE(), 'PHAM MINH CHAU');

INSERT INTO affiliate_network_dev.dbo.Policies (header,description,created_at) VALUES
	 (N'Chính sách Hoa Hồng Linh Hoạt',N'Chúng tôi áp dụng mô hình hoa hồng đa cấp cho các đối tác tiếp thị liên kết. Mức hoa hồng sẽ tăng dần từ 5% đến 15% tùy theo số lượng đơn hàng thành công mà bạn mang lại. Điều này khuyến khích các đối tác tích cực và cam kết phát triển mạng lưới khách hàng.','2025-03-28 13:15:11.0600000'),
	 (N'Quy Tắc Đạo Đức Tiếp Thị',N'Các đối tác cam kết sử dụng các phương thức tiếp thị trung thực, minh bạch và không gây phiền toái cho người dùng. Nghiêm cấm sử dụng các chiến thuật lừa đảo, spam, hoặc nội dung gây nhầm lẫn. Vi phạm sẽ dẫn đến việc chấm dứt hợp đồng ngay lập tức.','2025-03-28 13:15:11.0600000'),
	 (N'Chính Sách Thanh Toán Nhanh Chóng',N'Thanh toán hoa hồng được thực hiện vào ngày 15 và 30 hàng tháng. Ngưỡng rút tiền tối thiểu là 100,000 VND. Chúng tôi hỗ trợ thanh toán qua nhiều kênh như chuyển khoản ngân hàng, ví điện tử MOMO, và ZaloPay để mang lại sự thuận tiện tối đa.','2025-03-28 13:15:11.0600000'),
	 (N'Chính Sách Bảo Vệ Thương Hiệu',N'Các đối tác không được sử dụng logo, thương hiệu của chúng tôi mà không có sự cho phép bằng văn bản. Mọi nội dung tiếp thị phải được xem xét và phê duyệt trước khi đăng tải. Chúng tôi cam kết bảo vệ hình ảnh và uy tín của thương hiệu.','2025-03-28 13:15:11.0600000'),
	 (N'Quy Định Về Nội Dung Tiếp Thị',N'Nội dung tiếp thị phải chân thực, rõ ràng và không gây hiểu lầm về sản phẩm. Khuyến khích các đối tác tạo ra nội dung có giá trị, cung cấp thông tin hữu ích và đánh giá khách quan để xây dựng niềm tin với người tiêu dùng.','2025-03-28 13:15:11.0600000'),
	 (N'Chính Sách Theo Dõi và Giám Sát',N'Chúng tôi sử dụng các công cụ theo dõi hiện đại để ghi nhận mọi đơn hàng và nguồn truy cập. Các đối tác được cung cấp tài khoản truy cập để theo dõi số liệu hiệu quả tiếp thị của mình một cách minh bạch và chi tiết.','2025-03-28 13:15:11.0600000'),
	 (N'Quy Tắc Sử Dụng Liên Kết',N'Mỗi đối tác sẽ nhận được mã liên kết duy nhất. Nghiêm cấm việc can thiệp vào mã liên kết của người khác hoặc sử dụng các phương thức gian lận để tăng số lượng truy cập. Phát hiện vi phạm sẽ dẫn đến ngừng hợp tác ngay lập tức.','2025-03-28 13:15:11.0600000'),
	 (N'Chính Sách Bảo Mật Thông Tin',N'Chúng tôi cam kết bảo vệ tuyệt đối thông tin cá nhân của đối tác và khách hàng. Mọi dữ liệu được mã hóa và tuân thủ các quy định về bảo vệ dữ liệu cá nhân của Việt Nam. Các đối tác có trách nhiệm bảo mật thông tin trong quá trình tiếp thị.','2025-03-28 13:15:11.0600000'),
	 (N'Chính Sách Đào Tạo và Hỗ Trợ',N'Chúng tôi cung cấp miễn phí các khóa đào tạo về chiến lược tiếp thị liên kết, sử dụng công cụ, và các kỹ năng cần thiết. Hỗ trợ kỹ thuật được cung cấp 24/7 thông qua email, chat trực tuyến và đường dây nóng.','2025-03-28 13:15:11.0600000'),
	 (N'Quy Định Về Sản Phẩm Được Tiếp Thị',N'Các đối tác chỉ được phép tiếp thị các sản phẩm được chính thức phê duyệt. Nghiêm cấm tiếp thị các sản phẩm vi phạm pháp luật, đạo đức, hoặc có nguy cơ gây hại cho người tiêu dùng. Danh mục sản phẩm được cập nhật định kỳ và thông báo đến các đối tác.','2025-03-28 13:15:11.0600000');

INSERT INTO affiliate_network_dev.dbo.Campaigns (
    camp_id, advertiser_code, camp_name, description, start_date, end_date, 
    balance, product_url, tracking_params, reject_reason, cate_id, camp_status, concurrency_stamp
) VALUES
(1, 'ADV002', N'Shopee - Siêu Sale Tháng 3', N'Chương trình giảm giá lớn trên Shopee, hoàn tiền lên đến 10%', 
 '2025-03-01', '2025-03-31', 50000000.00, 'https://shopee.vn', 'utm_source=affiliate&utm_medium=cpa', 
 NULL, NULL, 1, DEFAULT),

(2, 'ADV002', N'Lazada - Mua Sắm Hoàn Tiền', N'Nhận ưu đãi hoàn tiền 8% khi mua sắm tại Lazada qua liên kết này', 
 '2025-03-05', '2025-04-05', 30000000.00, 'https://lazada.vn', 'utm_source=affiliate&utm_medium=cps', 
 NULL, NULL, 1, DEFAULT),

(3, 'ADV002', N'Tiki - Deal Hot Công Nghệ', N'Ưu đãi đặc biệt cho sản phẩm công nghệ trên Tiki, hoàn tiền 5%', 
 '2025-03-10', '2025-04-10', 40000000.00, 'https://tiki.vn', 'utm_source=affiliate&utm_medium=cpa', 
 NULL, NULL, 1, DEFAULT),

(4, 'ADV002', N'Sendo - Thời Trang Xuân Hè', N'Mua sắm thời trang mùa xuân hè, giảm giá đến 50%', 
 '2025-03-15', '2025-04-15', 20000000.00, 'https://sendo.vn', 'utm_source=affiliate&utm_medium=cps', 
 NULL, NULL, 1, DEFAULT),

(5, 'ADV002', N'VNTrip - Du Lịch Hè 2025', N'Đặt phòng khách sạn, combo du lịch giảm đến 30%', 
 '2025-03-20', '2025-05-20', 60000000.00, 'https://vntrip.vn', 'utm_source=affiliate&utm_medium=cpa', 
 NULL, NULL, 1, DEFAULT);

INSERT INTO affiliate_network_dev.dbo.Offers (
    camp_id, pricing_model, offer_description, step_info, start_date, end_date, 
    bid, budget, commission_rate, order_return_time, img_url, concurrency_stamp
) VALUES
(1, 'CPA', N'Hoa hồng 10% cho đơn hàng trên Shopee', N'Người dùng nhấp vào link, đặt hàng trên Shopee và hoàn tất thanh toán', 
 '2025-03-01', '2025-03-31', 2000.00, 20000000.00, 10.0, N'7 ngày', 'https://cdn.shopee.vn/banner.jpg', DEFAULT),

(2, 'CPS', N'Hoàn tiền 8% cho đơn hàng Lazada', N'Người dùng nhấp vào link, đặt hàng trên Lazada và thanh toán thành công', 
 '2025-03-05', '2025-04-05', 1500.00, 15000000.00, 8.0, N'7 ngày', 'https://cdn.lazada.vn/banner.jpg', DEFAULT),

(3, 'CPA', N'Mua sản phẩm công nghệ trên Tiki, hoàn tiền 5%', N'Người dùng nhấp vào link, mua sản phẩm công nghệ tại Tiki và hoàn tất thanh toán', 
 '2025-03-10', '2025-04-10', 2500.00, 20000000.00, 5.0, N'10 ngày', 'https://cdn.tiki.vn/banner.jpg', DEFAULT),

(4, 'CPS', N'Giảm giá 50% thời trang Sendo', N'Người dùng nhấp vào link, mua sản phẩm thời trang tại Sendo', 
 '2025-03-15', '2025-04-15', 1800.00, 10000000.00, 6.5, N'7 ngày', 'https://cdn.sendo.vn/banner.jpg', DEFAULT),

(5, 'CPA', N'Giảm giá 30% đặt phòng khách sạn VNTrip', N'Người dùng đặt phòng khách sạn qua VNTrip và hoàn tất thanh toán', 
 '2025-03-20', '2025-05-20', 3000.00, 30000000.00, 7.0, N'14 ngày', 'https://cdn.vntrip.vn/banner.jpg', DEFAULT);

INSERT INTO affiliate_network_dev.dbo.CampaignImages (
    camp_id, img_url, added_at
) VALUES
(1, 'https://cdn.shopee.vn/banner1.jpg', '2025-03-01'),
(1, 'https://cdn.shopee.vn/banner2.jpg', '2025-03-02'),
(2, 'https://cdn.lazada.vn/banner1.jpg', '2025-03-05'),
(2, 'https://cdn.lazada.vn/banner2.jpg', '2025-03-06'),
(3, 'https://cdn.tiki.vn/banner1.jpg', '2025-03-10'),
(4, 'https://cdn.sendo.vn/banner1.jpg', '2025-03-15'),
(4, 'https://cdn.sendo.vn/banner2.jpg', '2025-03-16'),
(5, 'https://cdn.vntrip.vn/banner1.jpg', '2025-03-20'),
(5, 'https://cdn.vntrip.vn/banner2.jpg', '2025-03-21'),
(5, 'https://cdn.vntrip.vn/banner3.jpg', '2025-03-22');

INSERT INTO affiliate_network_dev.dbo.PublisherOffers (offer_id, publisher_code, joining_date, approved_date, reject_reason, status)
VALUES
    (6, 'PUB009', GETDATE(), GETDATE(), NULL, 2),
    (6, 'PUB010', GETDATE(), GETDATE(), NULL, 2),
    (6, 'PUB011', GETDATE(), GETDATE(), NULL, 2),
    (7, 'PUB012', GETDATE(), GETDATE(), NULL, 2),
    (7, 'PUB013', GETDATE(), GETDATE(), NULL, 2),
    (7, 'PUB014', GETDATE(), GETDATE(), NULL, 2),
    (8, 'PUB015', GETDATE(), GETDATE(), NULL, 2),
    (8, 'PUB014', GETDATE(), GETDATE(), NULL, 2),
    (8, 'PUB013', GETDATE(), GETDATE(), NULL, 2),
    (9, 'PUB012', GETDATE(), GETDATE(), NULL, 2),
    (9, 'PUB011', GETDATE(), GETDATE(), NULL, 2),
    (9, 'PUB009', GETDATE(), GETDATE(), NULL, 2),
    (10, 'PUB011', GETDATE(), GETDATE(), NULL, 2),
    (10, 'PUB010', GETDATE(), GETDATE(), NULL, 2);

INSERT INTO affiliate_network_dev.dbo.BatchPayments (transaction_id, from_account, amount, beneficiary_name, beneficiary_account, reason, beneficiary_bank_code, beneficiary_bank_name, [date])
VALUES 
(1, '19073926783015', 1000000, 'Nguyen Van A', '123456789', 'Payment for services', '001', 'Vietcombank', GETDATE()),
(2, '19073926783015', 2000000, 'Tran Thi B', '987654321', 'Salary payment', '002', 'VietinBank', GETDATE()),
(3, '19073926783015', 1500000, 'Le Van C', '112233445', 'Loan repayment', '003', 'BIDV', GETDATE()),
(4, '19073926783015', 500000, 'Pham Thi D', '223344556', 'Payment for goods', '004', 'Agribank', GETDATE()),
(5, '19073926783015', 2500000, 'Hoang Van E', '334455667', 'Bonus payment', '005', 'Techcombank', GETDATE()),
(6, '19073926783015', 3000000, 'Nguyen Thi F', '445566778', 'Insurance payment', '006', 'MB Bank', GETDATE()),
(7, '19073926783015', 4000000, 'Tran Van G', '556677889', 'Rent payment', '007', 'Sacombank', GETDATE()),
(8, '19073926783015', 3500000, 'Le Thi H', '667788990', 'Utility payment', '008', 'ACB', GETDATE()),
(9, '19073926783015', 1800000, 'Pham Van I', '778899001', 'Payment for services', '009', 'VPBank', GETDATE()),
(10, '19073926783015', 2200000, 'Hoang Thi J', '889900112', 'Salary payment', '010', 'HDBank', GETDATE()),
(11, '19073926783015', 1600000, 'Nguyen Van K', '990011223', 'Loan repayment', '011', 'TPBank', GETDATE()),
(12, '19073926783015', 540000, 'Tran Thi L', '001122334', 'Payment for goods', '012', 'VIB', GETDATE()),
(13, '19073926783015', 2550000, 'Le Van M', '112233445', 'Bonus payment', '013', 'SHB', GETDATE()),
(14, '19073926783015', 3005000, 'Pham Thi N', '223344556', 'Insurance payment', '014', 'Eximbank', GETDATE()),
(15, '19073926783015', 4050000, 'Hoang Van O', '334455667', 'Rent payment', '015', 'LienVietPostBank', GETDATE()),
(16, '19073926783015', 3550000, 'Nguyen Thi P', '445566778', 'Utility payment', '016', 'SCB', GETDATE()),
(17, '19073926783015', 1850000, 'Tran Van Q', '556677889', 'Payment for services', '017', 'OceanBank', GETDATE()),
(18, '19073926783015', 2250000, 'Le Thi R', '667788990', 'Salary payment', '018', 'SeABank', GETDATE()),
(19, '19073926783015', 1650000, 'Pham Van S', '778899001', 'Loan repayment', '019', 'OCB', GETDATE()),
(20, '19073926783015', 550000, 'Hoang Thi T', '889900112', 'Payment for goods', '020', 'BacABank', GETDATE());

INSERT INTO affiliate_network_dev.dbo.Categories (
    cate_id, cate_name, description
) VALUES
(1, 'E-commerce', 'Các chương trình tiếp thị liên kết liên quan đến mua sắm trực tuyến, sàn thương mại điện tử.'),
(2, 'Finance & Investment', 'Tiếp thị liên kết trong lĩnh vực tài chính, ngân hàng, bảo hiểm, đầu tư.'),
(3, 'Health & Wellness', 'Tiếp thị sản phẩm và dịch vụ liên quan đến sức khỏe, làm đẹp, thực phẩm chức năng.'),
(4, 'Technology & Software', 'Các chiến dịch tiếp thị phần mềm, SaaS, hosting, công nghệ số.'),
(5, 'Travel & Hospitality', 'Tiếp thị liên kết cho ngành du lịch, khách sạn, đặt vé máy bay, tour du lịch.'),
(6, 'Education & Online Courses', 'Tiếp thị các khóa học trực tuyến, nền tảng giáo dục số, sách điện tử.'),
(7, 'Gaming & Entertainment', 'Tiếp thị các sản phẩm liên quan đến game, streaming, giải trí số.'),
(8, 'Fashion & Apparel', 'Tiếp thị liên kết trong ngành thời trang, giày dép, phụ kiện.'),
(9, 'Automotive', 'Tiếp thị trong lĩnh vực xe cộ, bảo dưỡng, phụ kiện xe hơi.'),
(10, 'Home & Living', 'Tiếp thị các sản phẩm nội thất, gia dụng, thiết bị thông minh cho nhà cửa.');

INSERT INTO affiliate_network_dev.dbo.Subscriptions (
    sub_id, sub_name, description, sub_price, duration
) VALUES
(1, 'Gói Cơ Bản', 'Cho phép tạo tối đa 5 chiến dịch. Không có ưu tiên hiển thị.', 500000.00, '1 tháng'),
(2, 'Gói Tiêu Chuẩn', 'Tạo tối đa 15 chiến dịch. Chiến dịch được ưu tiên hiển thị trong danh mục.', 1500000.00, '1 tháng'),
(3, 'Gói Chuyên Nghiệp', 'Tạo tối đa 30 chiến dịch. Chiến dịch được ghim lên vị trí đầu trong danh mục.', 3000000.00, '1 tháng'),
(4, 'Gói Doanh Nghiệp', 'Không giới hạn số lượng chiến dịch. Chiến dịch luôn được ưu tiên hiển thị.', 5000000.00, '1 tháng'),
(5, 'Gói VIP', 'Không giới hạn số lượng chiến dịch. Chiến dịch được hiển thị nổi bật trên toàn hệ thống.', 10000000.00, '1 tháng');

INSERT INTO affiliate_network_dev.dbo.Wallets (
    balance, is_active, user_code
) VALUES
-- Advertisers
(0.00, 1, 'ADV002'),
(0.00, 1, 'ADV003'),
(0.00, 1, 'ADV004'),
(0.00, 1, 'ADV005'),
(0.00, 1, 'ADV006'),
(0.00, 1, 'ADV007'),
(0.00, 1, 'ADV008'),
(0.00, 1, 'ADV009'),
(0.00, 1, 'ADV010'),
(0.00, 1, 'ADV011'),
-- Publishers
(0.00, 1, 'PUB012'),
(0.00, 1, 'PUB013'),
(0.00, 1, 'PUB014'),
(0.00, 1, 'PUB015'),
(0.00, 1, 'PUB016'),
(0.00, 1, 'PUB017'),
(0.00, 1, 'PUB018'),
(0.00, 1, 'PUB019'),
(0.00, 1, 'PUB020'),
(0.00, 1, 'PUB021'),
(0.00, 1, 'PUB022'),
(0.00, 1, 'PUB023'),
(0.00, 1, 'PUB024'),
(0.00, 1, 'PUB025'),
(0.00, 1, 'PUB026');
