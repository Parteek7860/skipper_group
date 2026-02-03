document.addEventListener("DOMContentLoaded", function () {

  // Delay init so layout/images settle
  setTimeout(() => {
    gsap.registerPlugin(ScrollTrigger);
    ScrollTrigger.refresh();
    initEngineeringAnimations();
  }, 200);

});

function initEngineeringAnimations() {

  let width = window.innerWidth;
  if (width < 1001) return;

  // Clear inline styles
  gsap.set(".eng-wrapper", { clearProps: "all" });
  gsap.set(".home_infra", { clearProps: "all" });

  /* ===============================
     1. TOP HORIZONTAL LINE
  =============================== */
  gsap.set(".top-line-svg line", {
    strokeDasharray: 100,
    strokeDashoffset: 100
  });

  gsap.to(".top-line-svg line", {
    strokeDashoffset: 0,
    ease: "power2.out",
    scrollTrigger: {
      trigger: ".about_block2",
      start: "center center",
      end: "+=400",
      scrub: 1.5
    }
  });

  /* ===============================
     2. TOWER IMAGE
  =============================== */
  gsap.fromTo(".tower-right",
    { opacity: 0, x: 50 },
    {
      opacity: 1,
      x: 0,
      ease: "power2.out",
      duration: 0.8,
      scrollTrigger: {
        trigger: ".top-line-svg",
        start: "top 65%",
        toggleActions: "play none none reverse"
      }
    }
  );

  /* ===============================
     3. SECOND VERTICAL LINE
  =============================== */
  gsap.set(".second-vertical-line-svg line", {
    strokeDasharray: 200,
    strokeDashoffset: 200
  });

  gsap.to(".second-vertical-line-svg line", {
    strokeDashoffset: 0,
    ease: "power2.out",
    scrollTrigger: {
      trigger: ".tower-right",
      start: "center 60%",
      end: "+=350",
      scrub: 1.5
    }
  });

  /* ===============================
     4. DOT + HORIZONTAL LINE 2
  =============================== */
  gsap.fromTo(".second-dot",
    { opacity: 0 },
    {
      opacity: 1,
      duration: 0.4,
      scrollTrigger: {
        trigger: ".second-vertical-line-svg",
        start: "bottom 60%",
        end: "+=200",
        scrub: 1,
        onEnter: () => {
          gsap.fromTo(".horizontal-line2",
            { width: 0 },
            {
              width: "15%",
              duration: 0.6,
              ease: "power2.out"
            }
          );
        },
        onLeaveBack: () => {
          gsap.to(".horizontal-line2", {
            width: 0,
            duration: 0.4,
            ease: "power2.in"
          });
        }
      }
    }
  );

  /* ===============================
     5. MAIN ENGINEERING PIN
  =============================== */
  let engTimeline = gsap.timeline({
    scrollTrigger: {
      trigger: ".eng-wrapper",
      start: "top 15%",
      end: "+=450",
      scrub: 1,
      pin: true,
      pinSpacing: true,
      anticipatePin: 1
    }
  });

  engTimeline
    .fromTo(".engineering-image",
      { clipPath: "inset(0 0% 0 100%)" },
      { clipPath: "inset(0 0% 0 0%)", ease: "power2.out" },
      0.2
    )
    .fromTo(".engineering-bg",
      { scaleY: 0 },
      { scaleY: 1, ease: "power1.out", transformOrigin: "bottom" },
      0.2
    )
    .fromTo(".eng_text1",
      { opacity: 0, y: 40 },
      { opacity: 1, y: 0, ease: "power2.out" },
      0.6
    )
    .fromTo(".eng_text2",
      { opacity: 0, y: 40 },
      { opacity: 1, y: 0, ease: "power2.out" },
      0.9
    );

  /* ===============================
     6. SECOND VERTICAL LINE 2
  =============================== */
  ScrollTrigger.matchMedia({

    "(min-width: 2500px)": function () {
      gsap.fromTo(".second-vertical-line2",
        { height: 0 },
        {
          height: "660px",
          scrollTrigger: {
            trigger: ".eng_text2",
            start: "bottom 65%",
            end: "+=350",
            scrub: 1.5
          }
        }
      );
    },

    "(max-width: 2499px)": function () {
      gsap.fromTo(".second-vertical-line2",
        { height: 0 },
        {
          height: "610px",
          scrollTrigger: {
            trigger: ".eng_text2",
            start: "bottom 65%",
            end: "+=350",
            scrub: 1.5
          }
        }
      );
    }

  });

  /* ===============================
     7. DOT 2
  =============================== */
  gsap.to(".second-dot2", {
    opacity: 1,
    duration: 0.6,
    scrollTrigger: {
      trigger: ".second-vertical-line2",
      start: "bottom 70%",
      end: "+=200",
      scrub: 1
    }
  });

  /* ===============================
     8. HORIZONTAL LINE 3
  =============================== */
  gsap.set(".horizontal-line3-svg line", {
    strokeDasharray: 200,
    strokeDashoffset: -200
  });

  gsap.to(".horizontal-line3-svg line", {
    strokeDashoffset: 0,
    ease: "power2.out",
    scrollTrigger: {
      trigger: ".second-dot2",
      start: "top 70%",
      end: "+=300",
      scrub: 1.5,
      fastScrollEnd: false
    }
  });

  /* ===============================
     9. INFRA BOX REVEALS
  =============================== */
  gsap.fromTo(".infra_box.right",
    { opacity: 0, x: 100 },
    {
      opacity: 1,
      x: 0,
      ease: "power2.out",
      scrollTrigger: {
        trigger: ".horizontal-line3-svg",
        start: "top 65%",
        end: "+=200",
        scrub: 1.1
      }
    }
  );

  gsap.fromTo(".infra_box.left",
    { opacity: 0, x: -100 },
    {
      opacity: 1,
      x: 0,
      ease: "power2.out",
      scrollTrigger: {
        trigger: ".horizontal-line3-svg",
        start: "top 60%",
        end: "+=200",
        scrub: 1.1
      }
    }
  );

  /* ===============================
     10. INFRA BOX CONTENT
  =============================== */
 document.querySelectorAll(".infra_box").forEach((box) => {

  // Force visible by default (important)
  gsap.set(box.querySelectorAll("h3, p, .read_icon, .infra_num"), {
    opacity: 1
  });

  const tl = gsap.timeline({
    scrollTrigger: {
      trigger: box,
      start: "top 70%",
      toggleActions: "play none none reverse"
    }
  });

  tl.from(box.querySelector("h3"), {
    y: 20,
    duration: 0.3,
    ease: "power2.out"
  })
  .from(box.querySelector("p"), {
    y: 20,
    duration: 0.4,
    ease: "power2.out"
  }, "-=0.15")
  .from(box.querySelector(".read_icon"), {
    y: 20,
    duration: 0.4,
    ease: "power2.out"
  }, "-=0.2")
  .from(box.querySelector(".infra_num"), {
    y: 20,
    duration: 0.4,
    ease: "power2.out"
  }, "-=0.2");

});


}
