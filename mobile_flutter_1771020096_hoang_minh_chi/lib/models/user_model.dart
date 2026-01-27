class UserModel {
  final String userId;
  final String fullName;
  final String email;
  final String role;
  final double walletBalance;
  final String tier;
  final String token;

  UserModel({
    required this.userId,
    required this.fullName,
    required this.email,
    required this.role,
    required this.walletBalance,
    required this.tier,
    required this.token,
  });

  factory UserModel.fromJson(Map<String, dynamic> json) {
    return UserModel(
      userId: json['userId'] ?? '',
      fullName: json['fullName'] ?? '',
      email: json['email'] ?? '',
      role: json['role'] ?? 'Member',
      walletBalance: (json['walletBalance'] as num?)?.toDouble() ?? 0.0,
      tier: json['tier'] ?? 'Standard',
      token: json['token'] ?? '',
    );
  }
}