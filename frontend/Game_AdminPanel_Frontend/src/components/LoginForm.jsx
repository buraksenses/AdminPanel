function LoginForm({
  handleSubmit,
  setUsername,
  setPassword,
  setConfirmPassword,
  setEmail,
  isLogin,
}) {
  return (
    <div>
      <form className="auth-form" onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Username"
          onChange={(e) => setUsername(e.target.value)}
          required
        />
        <input
          type="password"
          placeholder="Password"
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        {!isLogin && (
          <>
            <input
              type="password"
              placeholder="Confirm Password"
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
            />
            <input
              type="email"
              placeholder="Email"
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </>
        )}
        <button type="submit">{isLogin ? "Login" : "Register"}</button>
      </form>
    </div>
  );
}

export default LoginForm;
