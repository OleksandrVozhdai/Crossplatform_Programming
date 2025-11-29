import React from "react";
import img1 from "../assets/1.png";
import img2 from "../assets/2.png";
import img3 from "../assets/3.png";
import domino from "../assets/domino-bg.png";
import logo from "../assets/logo.png";
import lupa from "../assets/lupa.png";
import stars from "../assets/stars.png";
import time from "../assets/feature-time.png";
import custom from "../assets/feature-custom.png";

const HomePage = ({ isAuthenticated = false, userName = "" }) => {
  const handleViewAll = () => {
    window.location.href = "/Cases/ChoosePage";
  };

  const handleLogout = () => {
    window.location.href = "/Account/Logout"; 
  };

  const handleSignIn = () => {
    window.location.href = "/Account/Login";
  };

  const publicUrl = process.env.PUBLIC_URL || "";

  return (
    <>
      <div className="nav-upper-panel">
        <div className="nav-upper-panel-ov">
          <div className="nav-upper-panel-bar">
            <p>Made in Ukraine</p>
            <p>Stay informed with real-time COVID-19 monitoring worldwide.</p>
            <a>
              <b>Public Health Support</b>
            </a>
          </div>
        </div>
      </div>

      <header>
        <div className="header-bar">
          <div className="logo-section">
            <a className="logo" href="/">
              <img
                src={logo}
                alt="Logo"
                style={{ display: "block" }}
              />
            </a>

            <div className="nav-buttons">
              <div className="shop-btn" onClick={handleViewAll} role="button" tabIndex={0}>
                <p> View All </p>
                <svg
                  width="25"
                  height="25"
                  viewBox="0 0 25 25"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                  className="svg-circle-btn"
                >
                  <circle cx="12.5" cy="12.5" r="12.5" className="svg-circle" />
                  <path
                    d="M20.7071 13.7071C21.0976 13.3166 21.0976 12.6834 20.7071 12.2929L14.3431 5.92893C13.9526
                                5.53841 13.3195 5.53841 12.9289 5.92893C12.5384 6.31946 12.5384 6.95262 12.9289 7.34315L18.5858
                                13L12.9289 18.6569C12.5384 19.0474 12.5384 19.6805 12.9289 20.0711C13.3195 20.4616 13.9526 20.4616
                                14.3431 20.0711L20.7071 13.7071ZM5 14H20V12H5V14Z"
                    className="svg-arrow"
                  />
                </svg>
              </div>

              <a href="/faqs"> FAQs </a>
              <a href="/report"> Submit a Report </a>
            </div>
          </div>

          <div className="contact-section">
            <a href="tel:+18882321901"> + 1 (888) 232-1901 </a>
            <div className="account-btn">
              <a style={{ height: 32, display: "inline-flex", alignItems: "center" }}>
                <svg width="32" height="32" viewBox="0 0 32 32" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <path
                    d="M16.0648 27.5603C12.1984 27.5603 8.78048 25.5807
                        6.78542 22.6114C6.83181 19.5182 12.9717 17.817 16.0648 17.817C19.1579 17.817
                        25.2977 19.5182 25.3441 22.6114C23.3491 25.5807 19.9312 27.5603 16.0648 27.5603ZM16.0648
                        5.59921C17.2953 5.59921 18.4754 6.08803 19.3455 6.95814C20.2156 7.82825 20.7044 9.00837
                        20.7044 10.2389C20.7044 11.4694 20.2156 12.6495 19.3455 13.5196C18.4754 14.3897 17.2953
                        14.8786 16.0648 14.8786C14.8342 14.8786 13.6541 14.3897 12.784 13.5196C11.9139 12.6495
                        11.4251 11.4694 11.4251 10.2389C11.4251 9.00837 11.9139 7.82825 12.784 6.95814C13.6541 6.08803
                        14.8342 5.59921 16.0648 5.59921ZM16.0648 0.959534C14.0338 0.959534 12.0227 1.35956 10.1463
                        2.13678C8.26997 2.914 6.56506 4.05319 5.12895 5.4893C2.22859 8.38966 0.599182 12.3234 0.599182
                        16.4251C0.599182 20.5268 2.22859 24.4606 5.12895 27.3609C6.56506 28.7971 8.26997 29.9362 10.1463
                        30.7135C12.0227 31.4907 14.0338 31.8907 16.0648 31.8907C20.1665 31.8907 24.1002 30.2613 27.0006
                        27.3609C29.9009 24.4606 31.5304 20.5268 31.5304 16.4251C31.5304 7.87265 24.5708 0.959534 16.0648
                        0.959534Z"
                    fill="#C0C0A8"
                  />
                </svg>
              </a>

              {isAuthenticated ? (
                <>
                  <span className="sing-text">Hello, {userName}!</span>
                  <button
                    type="button"
                    className="sing-text logout-btn"
                    onClick={handleLogout}
                  >
                    Logout
                  </button>
                </>
              ) : (
                <button
                  type="button"
                  className="sing-text signin-btn"
                  onClick={handleSignIn}
                >
                  Sign in
                </button>
              )}
            </div>
          </div>
        </div>

        <span className="bottom-header-line" />
      </header>

      <main>
        <section className="hero">
          <div className="hero-wrapper">
            <div
              className="box-content"
              style={{
                backgroundImage: `url(${publicUrl}/images/bg.png)`,
              }}
            >
              <p className="header-text slide-up-text">COVID-19 Monitor</p>
              <h1>
                <span className="text-wrapper">
                  <span style={{ "--animation-order": 1, "marginRight":10  }} className="slide-up">
                    {" "}
                    Where{" "}
                  </span>
                </span>
                <span className="text-wrapper">
                  <span style={{ "--animation-order": 2, "marginRight":10  }} className="slide-up">
                    data
                  </span>
                </span>
                <span className="text-wrapper">
                  <span style={{ "--animation-order": 3, "marginRight":10  }} className="slide-up">
                    meets
                  </span>
                </span>
                <span className="text-wrapper">
                  <span style={{ "--animation-order": 4 }} className="slide-up">
                    prevention
                  </span>
                </span>
              </h1>

              <p className="desc slide-up-text">
                Track, analyze, and respond to COVID-19 outbreaks faster with
                real-time surveillance and predictive analytics.
              </p>

              <a href="/Cases/Index" className="btn-wrapper btn-override">
                <div className="btn">
                  <p>Explore Dashboard</p>
                </div>
              </a>

              <div className="rating-block scroll-trigger animate--slide-in">
                <img
                  style={{ "--animation-order": 1 }}
                  className="stars-icon desc slide-up-text"
                  src={stars}
                  alt="5 stars rating"
                />
                <p className="stars-desc desc">
                  <span className="slide-up-text" style={{ "--animation-order": 2 }}>
                    Trusted by healthcare professionals
                  </span>
                  <span className="slide-up-text" style={{ "--animation-order": 3 }}>
                    {" "}
                    and research institutions worldwide.
                  </span>
                </p>
              </div>
            </div>

            <img
              className="falling-box-1"
              loading="lazy"
              alt=""
              src={img2}
            />
            <img
              className="falling-box-2"
              loading="lazy"
              alt=""
              src={lupa}
            />
            <img
              className="falling-box-3"
              loading="lazy"
              alt=""
              src={img3}
            />
            <img
              className="falling-box-4"
              loading="lazy"
              alt=""
              src={img1}
            />
            <img
              className="animated-bg"
              loading="lazy"
              alt=""
              src={domino}
            />
          </div>

          <div className="feature-panel">
            <div className="feature-customizable-box" style={{ "--animation-order": 2 }}>
              <img className="stars-icon" src={custom} alt="feature" />
              <p className="feature-header"> Fully adaptable</p>
              <p className="feature-description"> Monitor any region. </p>
            </div>

            <div className="feature-time-box" style={{ "--animation-order": 1 }}>
              <img className="stars-icon" src={time} alt="feature" />
              <p className="feature-header"> Quickly access live data</p>
              <p className="feature-description"> With our intuitive outbreak dashboard. </p>
            </div>
          </div>
        </section>
      </main>
    </>
  );
};

export default HomePage;
