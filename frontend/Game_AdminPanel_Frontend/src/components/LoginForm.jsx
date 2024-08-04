export default function LoginForm({
                     handleSubmit,
                     username,
                     setUsername,
                     password,
                     setPassword,
                     confirmPassword,
                     setConfirmPassword,
                     email,
                     setEmail,
                     isLogin,
                   }) {
  return (
      <div>
        <form className="auth-form" onSubmit={handleSubmit}>
          <input
              type="text"
              placeholder="Username"
              value={username || ''}
              onChange={(e) => setUsername(e.target.value)}
              required
          />
          <input
              type="password"
              placeholder="Password"
              value={password || ''}
              onChange={(e) => setPassword(e.target.value)}
              required
          />
          {!isLogin && (
              <>
                <input
                    type="password"
                    placeholder="Confirm Password"
                    value={confirmPassword || ''}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                    required
                />
                <input
                    type="email"
                    placeholder="Email"
                    value={email || ''}
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
