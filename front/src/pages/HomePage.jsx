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
import Header from "../components/Header";

const HomePage = ({ isAuthenticated = false, userName = "" }) => {

  const publicUrl = process.env.PUBLIC_URL || "";

  return (
    <>
      <Header/>

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
