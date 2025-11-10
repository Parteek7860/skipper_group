document.addEventListener("DOMContentLoaded", function () {
  gsap.registerPlugin(ScrollTrigger);
  let width = window.innerWidth;

  if (width >= 1001) {
    // Clear inline styles
    gsap.set(".eng-wrapper", { clearProps: "all" });
    gsap.set(".home_infra", { clearProps: "all" });

    // 1. Top Horizontal Line
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
        end: "+=600",
        scrub: 1
      }
    });

    // 2. Tower Image Fade In
    gsap.fromTo(".tower-right",
      { opacity: 0, x: 50 },
      {
        opacity: 1,
        x: 0,
        ease: "power2.out",
        duration: 0.6,
        scrollTrigger: {
          trigger: ".top-line-svg",
          start: "top center",
          toggleActions: "play none none reverse"
        }
      }
    );

    // 3. Vertical Line (Second Line)
    gsap.set(".second-vertical-line-svg line", {
      strokeDasharray: 200,
      strokeDashoffset: 200
    });

    gsap.to(".second-vertical-line-svg line", {
      strokeDashoffset: 0,
      ease: "power2.out",
      scrollTrigger: {
        trigger: ".tower-right",
        start: "center+=50 center",
        end: "+=300",
        scrub: 1
      }
    });

    // 4. Dot Fade-In
    gsap.fromTo(".second-dot",
      { opacity: 0 },
      {
        opacity: 1,
        duration: 0.3,
        scrollTrigger: {
          trigger: ".second-vertical-line-svg",
          start: "bottom center",
          end: "+=100",
          toggleActions: "play none none reverse",
          onEnter: () => {
            // 5. Horizontal Line appears only after dot is visible
            gsap.fromTo(".horizontal-line2",
              { width: 0 },
              {
                width: "15%",
                ease: "power2.out",
                duration: 0.6
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


  // 5. MAIN ENGINEERING SECTION - PIN & ANIMATE
  let engTimeline = gsap.timeline({
    scrollTrigger: {
      trigger: ".eng-wrapper",
      start: "top 10%",
      end: "+=300",
      scrub: 2,
      pin: true,
      pinSpacing: true
    }
  });


  engTimeline
 

    .fromTo(".engineering-image", { clipPath: "inset(0 0% 0 100%)" }, { clipPath: "inset(0 0% 0 0%)" }, 0.2)
    .fromTo(".engineering-bg", { scaleY: 0 }, { scaleY: 1, ease: "power1.out", transformOrigin: "bottom" }, 0.2)
    .fromTo(".eng_text1", { opacity: 0, y: 40 }, { opacity: 1, y: 0, ease: "power2.out" }, 0.6)
    .fromTo(".eng_text2", { opacity: 0, y: 40 }, { opacity: 1, y: 0, ease: "power2.out" }, 0.9);

   // 6. Animate second-vertical-line2 (after .eng_text2 appears)
    // 10. Vertical line 2
    ScrollTrigger.matchMedia({
      // Screens above 2499px
      "(min-width: 2500px)": function () {
        gsap.fromTo(".second-vertical-line2",
          { height: 0 },
          {
            height: "660px",
            ease: "none",
            scrollTrigger: {
              trigger: ".eng_text2",
              start: "bottom center",
              end: "+=100",
              scrub: true,
            }
          }
        );
      },
    
      // Screens 2499px and below
      "(max-width: 2499px)": function () {
        gsap.fromTo(".second-vertical-line2",
          { height: 0 },
          {
            height: "610px",
            ease: "none",
            scrollTrigger: {
              trigger: ".eng_text2",
              start: "bottom center",
              end: "+=100",
              scrub: true,
            }
          }
        );
      }
    });

  // 7. Fade in .second-dot2 after the line reaches bottom
    // 11. Dot2
    gsap.to(".second-dot2", {
      opacity: 1,
      duration: 0.9,
      scrollTrigger: {
        trigger: ".second-vertical-line2",
        start: "top 0%",
        end: "+=100",
        toggleActions: "play none none reverse"
      }
    });
  
    // 12. Horizontal line 3
    gsap.set(".horizontal-line3-svg line", {
      strokeDasharray: 200,
      strokeDashoffset: -200  // 👈 Start from the right side
    });
    
    gsap.to(".horizontal-line3-svg line", {
      strokeDashoffset: 0,  // 👈 Animates back to 0 (reveals right to left)
      ease: "power2.out",
      scrollTrigger: {
        trigger: ".second-dot2",
        start: "top 30%",
        scrub: true,
     //   markers: true,
        toggleActions: "play none none reverse"
      }
    });

// 13. Infra box reveal - RIGHT first
gsap.fromTo(".infra_box.right",
  { opacity: 0, x: 100 },
  {
    opacity: 1,
    x: 0,
    ease: "power2.out",
    scrollTrigger: {
      trigger: ".horizontal-line3-svg line",
      start: "top 30%",
      end: "top 80%", // Smooth transition over scroll distance
      scrub: true,
      toggleActions: "play none none reverse"
    }
  }
);

// Then LEFT one after
gsap.fromTo(".infra_box.left",
  { opacity: 0, x: -100 },
  {
    opacity: 1,
    x: 0,
    ease: "power2.out",
    scrollTrigger: {
      trigger: ".horizontal-line3-svg line",
      start: "top 20%", // Trigger a little later than right box
      end: "top 80%",
      scrub: true,
      toggleActions: "play none none reverse"
    }
  }
);
 
  
  document.querySelectorAll(".infra_box").forEach((box) => {
    const heading = box.querySelector("h3");
    const paragraph = box.querySelector("p");
    const readMore = box.querySelector(".read_icon");
    const infra_num = box.querySelector(".infra_num");
  
    const tl = gsap.timeline({
      scrollTrigger: {
        trigger: box,
        start: "top 35%",
        toggleActions: "play none none reverse"
      }
    });
  
    tl.fromTo(heading,
      { opacity: 0, y: 20 },
      { opacity: 1, y: 0, duration: 0.3, ease: "power2.out" }
    )
    .fromTo(paragraph,
      { opacity: 0, y: 20 },
      { opacity: 1, y: 0, duration: 0.4, ease: "power2.out" },
      "-=0.1"
    )
    .fromTo(readMore,
      { opacity: 0, y: 20 },
      { opacity: 1, y: 0, duration: 0.5, ease: "power2.out" },
      "-=0.2"
    )
    .fromTo(infra_num,
      { opacity: 0, y: 20 },
      { opacity: 1, y: 0, duration: 0.6, ease: "power2.out" },
      "-=0.2"
    );
  });

} });