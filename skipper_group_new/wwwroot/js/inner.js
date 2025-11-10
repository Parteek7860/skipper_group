

gsap.registerPlugin(ScrollTrigger);
let width = window.innerWidth;
if (width >= 1001) {
  // 1. Top Horizontal Line (right to left)
  gsap.fromTo("#animatedLine",
    { attr: { x1: 600, x2: 600 } },
    {
      attr: { x1: 0, x2: 600 },
      ease: "none",
      scrollTrigger: {
        trigger: ".about_three",
        start: "top center",
        end: "top+=100 center",
        scrub: 2
      }
    });

  // 2. Slide/Reveal the vertical line wrapper


  // 3. Draw the vertical SVG line
  gsap.fromTo("#secondVerticalLine",
    { attr: { y2: 0 } },
    {
      attr: { y2: 550 },
      ease: "none",
      scrollTrigger: {
        trigger: ".about_three",
        start: "top+=220 center",
        end: "top+=350 center",
        scrub: 2
      }
    });

  // 4. Dot appears
  gsap.from(".inner_first_dot", {
    scale: 0,
    opacity: 0,
    transformOrigin: "center",
    ease: "back.out(1.7)",
    scrollTrigger: {
      trigger: ".about_three",
      start: "top+=360 center",
      end: "top+=420 center",
      scrub: 2
    }
  });

  // 5. Second Horizontal Line (left to right)
  gsap.fromTo("#animatedLine1",
    { attr: { x1: 0, x2: 0 } },
    {
      attr: { x2: 50 },
      ease: "none",
      scrollTrigger: {
        trigger: ".about_three",
        start: "top+=430 center",
        end: "top+=500 center",
        scrub: 2
      }
    });

  // 6. Final Element (.tower_about1) appears
  gsap.from(".towe_abou1", {
    opacity: 0,
    y: 50,
    ease: "power2.out",
    scrollTrigger: {
      trigger: ".about_three",
      start: "top+=510 center",
      end: "top+=700 center",
      scrub: 2
    }
  });


  // 7. Final Element (.tower_about1 text) appears
  gsap.from(".textp1", {
    opacity: 0,
    y: -50,
    ease: "power2.out",
    scrollTrigger: {
      trigger: ".about_three",
      start: "top+=510 center",
      end: "top+=700 center",
      scrub: 2
    }
  });


  // 8. animated line11 appears
  gsap.fromTo("#animatedLine11",
    { attr: { x1: 0, x2: 0 } },  // Line starts with no visible length
    {
      attr: { x2: 600 },         // Line draws from left to right
      ease: "none",
      scrollTrigger: {
        trigger: ".textp1",
        start: "top+=310 center",
        end: "top+=300 center",
        scrub: 2
      }
    });


  // 3. Draw the vertical SVG line
  gsap.fromTo("#secondVerticalLine11",
    { attr: { y2: 0 } },
    {
      attr: { y2: 550 },
      ease: "none",
      scrollTrigger: {
        trigger: "#animatedLine11",
        start: "top+=220 center",
        end: "top+=350 center",
        scrub: 2
      }
    });

  // 4. Dot appears
  gsap.from(".inner_first_dot11", {
    scale: 0,
    opacity: 0,
    transformOrigin: "center",
    ease: "back.out(1.7)",
    scrollTrigger: {
      trigger: "#secondVerticalLine11",
      start: "top+=360 center",
      end: "top+=420 center",
      scrub: 2
    }
  });

  // 5. Second Horizontal Line (left to right)
  gsap.fromTo("#animatedLine12",
    { attr: { x1: 50, x2: 50 } },  // Line starts with zero length at the right end
    {
      attr: { x1: 0 },             // x1 moves left, creating line from right to left
      ease: "none",
      scrollTrigger: {
        trigger: "#animatedLine12",
        start: "top+=30 center",
        end: "top+=100 center",
        scrub: 2
      }
    });




  gsap.fromTo("#animatedLine21",
    { attr: { x1: 600, x2: 600 } },
    {
      attr: { x1: 0, x2: 600 },
      ease: "none",
      scrollTrigger: {
        trigger: ".new_line_block2",
        start: "top+=220 center",
        end: "top+=350 center",
        scrub: 2
      }
    });

  // 2. Slide/Reveal the vertical line wrapper


  // 3. Draw the vertical SVG line
  gsap.fromTo("#secondVerticalLine22",
    { attr: { y2: 0 } },
    {
      attr: { y2: 550 },
      ease: "none",
      scrollTrigger: {
        trigger: ".new_line_block2",
        start: "top+=300 center",
        end: "top+=350 center",
        scrub: 2
      }
    });

  // 4. Dot appears
  gsap.from(".inner_first_dot23", {
    scale: 0,
    opacity: 0,
    transformOrigin: "center",
    ease: "back.out(1.7)",
    scrollTrigger: {
      trigger: ".new_line_block2",
      start: "top+=360 center",
      end: "top+=420 center",
      scrub: 2
    }
  });

  // 5. Second Horizontal Line (left to right)
  gsap.fromTo("#animatedLine24",
    { attr: { x1: 0, x2: 0 } },
    {
      attr: { x2: 50 },
      ease: "none",
      scrollTrigger: {
        trigger: ".new_line_block2",
        start: "top+=430 center",
        end: "top+=500 center",
        scrub: 2
      }
    });



  gsap.fromTo("#secondVerticalLine32",
    { attr: { y2: 0 } },
    {
      attr: { y2: 550 },
      ease: "none",
      scrollTrigger: {
        trigger: ".new_line_block3",
        start: "top+=200 center",
        end: "top+=250 center",
        scrub: 2
      }
    });

  // 4. Dot appears
  gsap.from(".inner_first_dot33", {
    scale: 0,
    opacity: 0,
    transformOrigin: "center",
    ease: "back.out(1.7)",
    scrollTrigger: {
      trigger: ".new_line_block3",
      start: "top+=360 center",
      end: "top+=320 center",
      scrub: 2
    }
  });


  // Scroll-linked movement for .icon_project
  gsap.to(".about-bg-icon1", {
    y: 500,
    ease: "none",
    scrollTrigger: {
      trigger: ".about_panel_six",
      start: "top 10%",
      end: "bottom+=500 top",
      ease: "power3.out",
      scrub: .5,
      //   markers: true // helps you see if it's being triggered
    }
  });


  document.addEventListener("DOMContentLoaded", () => {
    gsap.registerPlugin(ScrollTrigger);

    const svg = document.querySelector(".main-zigzag-line");
    const container = document.querySelector(".core_value_list");
    const svgContainer = document.querySelector(".svg_connectors");
    const items = document.querySelectorAll(".core_value_item");

    const leftX = 200;
    const rightX = 930;

    if (!svg || items.length === 0) return;

    const fullHeight = container.scrollHeight;
    svg.setAttribute("viewBox", `0 0 930 ${fullHeight}`);
    svgContainer.style.height = `${fullHeight}px`;

    setTimeout(() => {
      // Get vertical center Y of each item relative to container
      const midYs = Array.from(items).map(item => {
        return item.offsetTop + item.offsetHeight / 2;
      });

      let prevX = leftX;
      let prevY = midYs[0]; // first item vertical center

      for (let i = 1; i < items.length; i++) {
        const isEven = i % 2 === 0;
        const currX = isEven ? leftX : rightX;
        const currY = midYs[i];

        let d = "";
        let dotX, dotY;

        if (i === 1) {
          const yOffset = 180;
          const adjustedMidY = currY - yOffset;
          d = `M${prevX},${prevY} L${prevX},${adjustedMidY} L${currX},${adjustedMidY} L${currX},${currY}`;

          dotX = prevX;
          dotY = adjustedMidY;
        } else {
          // Normal zigzag segment: vertical down, horizontal across, vertical down
          const midY = (prevY + currY) / 2;
          d = `M${prevX},${prevY} L${prevX},${midY} L${currX},${midY} L${currX},${currY}`;

          dotX = prevX;
          dotY = midY;
        }

        // Create path element and set attributes
        const path = document.createElementNS("http://www.w3.org/2000/svg", "path");
        path.setAttribute("d", d);
        path.setAttribute("stroke", "#004A70");
        path.setAttribute("stroke-width", "1.1");
        path.setAttribute("fill", "none");
        path.classList.add("zigzag-segment", `seg-${i}`);
        svg.appendChild(path);

        // Create dot circle after path to be on top visually
        const dot = createDot(dotX, dotY, i);
        svg.appendChild(dot);
        animateDot(dot, items[i]);

        // Animate stroke drawing with GSAP ScrollTrigger
        const length = path.getTotalLength();
        gsap.set(path, {
          strokeDasharray: length,
          strokeDashoffset: length,
          opacity: 0.3,
        });

        gsap.to(path, {
          strokeDashoffset: 0,
          opacity: 1,
          duration: 1,
          scrollTrigger: {
            trigger: items[i],
            start: "top 85%",
            end: "top 80%",
            toggleActions: "play none none reverse",
          },
        });

        prevX = currX;
        prevY = currY;
      }
    }, 100);

    function createDot(x, y, index) {
      const dot = document.createElementNS("http://www.w3.org/2000/svg", "circle");
      dot.setAttribute("cx", x);
      dot.setAttribute("cy", y);
      dot.setAttribute("r", 12.5); // 35px diameter (radius = diameter / 2)
      dot.setAttribute("fill", "#004A70");
      dot.classList.add("dot", `dot-${index}`);
      return dot;
    }

    function animateDot(dot, triggerItem) {
      gsap.fromTo(
        dot,
        { opacity: 0, scale: 0.4 },
        {
          opacity: 1,
          scale: 1,
          scrollTrigger: {
            trigger: triggerItem,
            start: "top 85%",
            end: "top 80%",
            toggleActions: "play none none reverse",
          },
        }
      );
    }

    // Optional debug circle to visualize points (uncomment calls above to use)
    function createDebugCircle(x, y, color = "red") {
      const c = document.createElementNS("http://www.w3.org/2000/svg", "circle");
      c.setAttribute("cx", x);
      c.setAttribute("cy", y);
      c.setAttribute("r", 4);
      c.setAttribute("fill", color);
      svg.appendChild(c);
    }
  });

  document.addEventListener("DOMContentLoaded", () => {
    gsap.registerPlugin(ScrollTrigger);

    gsap.utils.toArray(".core_value_item").forEach((item) => {
      gsap.to(item, {
        opacity: 1,
        duration: 0.6,
        ease: "power1.out",
        scrollTrigger: {
          trigger: item,
          start: "top 80%", // when top of item hits 80% viewport height
          toggleActions: "play none none reverse",
        },
      });
    });
  });



  /* infra js start here---*/


  // 1. Top Horizontal Line (right to left)
  gsap.fromTo("#infraanimatedLine",
    { attr: { x1: 600, x2: 600 } },
    {
      attr: { x1: 0, x2: 600 },
      ease: "none",
      scrollTrigger: {
        trigger: ".infra_three",
        start: "top-=140 center",
        end: "top+=40 center",
        scrub: 2
      }
    });

  // 2. Slide/Reveal the vertical line wrapper


  // 3. Draw the vertical SVG line
  gsap.fromTo("#infrasecondVerticalLine",
    { attr: { y2: 0 } },
    {
      attr: { y2: 550 },
      ease: "none",
      scrollTrigger: {
        trigger: ".infra_three",
        start: "top+=100 center",
        end: "top+=350 center",
        scrub: 2
      }
    });

  // 4. Dot appears
  gsap.from(".infra_first_dot", {
    scale: 0,
    opacity: 0,
    transformOrigin: "center",
    ease: "back.out(1.7)",
    scrollTrigger: {
      trigger: ".infra_three",
      start: "top+=300 center",
      end: "top+=450 center",
      scrub: 2
    }
  });

  // 4. text  appears
  gsap.from(".infra_three_textpanel", {
    opacity: 0,
    transformOrigin: "center",
    ease: "none",
    scrollTrigger: {
      trigger: ".infra_first_dot",
      start: "top+=300 center",
      end: "top+=350 center",
      scrub: 2
    }
  });




  // 5. Draw the vertical SVG line
  gsap.fromTo("#infrasecondVerticalLine1",
    { attr: { y2: 0 } },
    {
      attr: { y2: 250 },
      ease: "none",
      scrollTrigger: {
        trigger: ".bottom-description",
        start: "top+=50 center",
        end: "top+=200 center",
        scrub: 2
      }
    });


  // 6. Dot appears
  gsap.from(".infra_first_dot1", {
    scale: 0,
    opacity: 0,
    transformOrigin: "center",
    ease: "back.out(1.7)",
    scrollTrigger: {
      trigger: "#infrasecondVerticalLine1",
      start: "top+=50 center",
      end: "top+=150 center",
      scrub: 2
    }
  });

  // 5. Draw the horizontal SVG line
  gsap.fromTo("#infraanimatedLine1",
    { attr: { x1: 600, x2: 600 } },
    {
      attr: { x1: 0, x2: 600 },
      ease: "none",
      scrollTrigger: {
        trigger: ".infra_first_dot1",
        start: "top+=100 center",
        end: "top+=100 center",
        scrub: 2
      }
    });

  // 6. Draw the vertical SVG line
  gsap.fromTo("#infrasecondVerticalLine2",
    { attr: { y2: 0 } },
    {
      attr: { y2: 250 },
      ease: "none",
      scrollTrigger: {
        trigger: "#infraanimatedLine1",
        start: "top+=320 center",
        end: "top+=300 center",
        scrub: 2
      }
    });


  // 6. Dot appears
  gsap.from(".infra_first_dot2", {
    scale: 0,
    opacity: 0,
    transformOrigin: "center",
    ease: "back.out(1.7)",
    scrollTrigger: {
      trigger: ".infra_first_dot2",
      start: "top+=280 center",
      end: "top+=300 center",
      scrub: 2
    }
  });

  // 6. Draw the vertical SVG line
  gsap.fromTo("#infrasecondVerticalLine5",
    { attr: { y2: 0 } },
    {
      attr: { y2: 250 },
      ease: "none",
      scrollTrigger: {
        trigger: ".plant_bottom_detail",
        start: "top+=120 center",
        end: "top+=200 center",
        scrub: 2
      }
    });

  // 6. Dot appears
  gsap.from(".infra_first_dot5", {
    scale: 0,
    opacity: 0,
    transformOrigin: "center",
    ease: "back.out(1.7)",
    scrollTrigger: {
      trigger: "#infrasecondVerticalLine5",
      start: "top+=200 center",
      end: "top+=300 center",
      scrub: 2
    }
  });



  // 4. Dot appears
  gsap.from(".infra_panel_block", {
    opacity: 0,
    transformOrigin: "center",
    ease: "none",
    scrollTrigger: {
      trigger: ".infra_first_dot2",
      start: "top+=180 center",
      end: "top+=300 center",
      scrub: 2,
      onEnter: () => {
        document.querySelectorAll('.infra_panel_block img').forEach(img => {
          img.classList.add('reveal-image2');
        });
      },
      onLeaveBack: () => {
        document.querySelectorAll('.infra_panel_block img').forEach(img => {
          img.classList.add('reveal-image2');
        });
      }
    }
  });



  gsap.timeline({
    scrollTrigger: {
      trigger: ".about_img_panel",
      start: "top 70%",
      end: "top 70%",
      toggleActions: "play none none reverse",
      //markers: true
    }
  })
    .to(".lft-icon", {
      duration: 1,
      x: "-70px",
      ease: "power2.out"
    }, 0)
    .to(".rght-icon", {
      duration: 1.5,
      x: "120px",
      ease: "power2.out"
    }, 0)
    .to(".tower-image2", {
      duration: 1,
      scrub: 2,
      opacity: 1,
      height: "100%",
      ease: "power2.out"
    }, 0.3);


    // Animate the vertical dashed line
    gsap.to(".milestone-line", {
        height: "100%",
        ease: "none",
        scrollTrigger: {
            trigger: ".milestone-grid ul",
            start: "top center",
            end: "bottom center",
            scrub: true,
        }
    });

    // Animate each milestone
    gsap.utils.toArray(".milestone-grid ul li").forEach((item) => {
        const dot = item.querySelector(".dot");
        const line = item.querySelector(".line");
        const box = item.querySelector(".miles-text");
        const year = item.querySelector(".milestone-meta h4");

        const tl = gsap.timeline({
            scrollTrigger: {
                trigger: item,
                start: "top 60%",
                toggleActions: "play none none reverse"
            }
        });

 tl.fromTo(year, { y: 30, opacity: 0 }, { y: 0, opacity: 1, duration: 0.4 })
  .to(dot, { scale: 1, opacity: 1, duration: 0.3, ease: "back.out(1.7)" }, "-=0.2")
  .to(line, { width: "200px", opacity: 1, duration: 0.4 }, "-=0.3")
  .to(box, { y: 0, opacity: 1, duration: 0.6, ease: "power2.out" }, "-=0.3");
    });




};








var swiper = new Swiper(".related-news", {
  pagination: {
    el: ".swiper-pagination",
    clickable: true,
  },
});


var swiper = new Swiper(".related-blog-slider", {
  slidesPerView: 2,
  spaceBetween: 30,
  pagination: {
    el: ".swiper-pagination",
    clickable: true,
  },
  // Responsive breakpoints
  breakpoints: {
    1024: {
      slidesPerView: 2,
      spaceBetween: 30
    },

    // when window width is >= 320px
    767: {
      slidesPerView: 1,
      spaceBetween: 10
    },

  }
});




// Annual revenue start
// $(".category-button").click(function() {
//   $(".category-button").removeClass("active");
//   $(this).addClass("active");
// });

// var $categoryButton = $(".category-button");
// var $pageContent = $(".page-content");

// $(".page-content")
//   .hide()
//   .first()
//   .show();

// $categoryButton.on("click", function(e) {
// var $category = $(this).data("target");
//   $pageContent
//     .hide()
//     .find('img').hide()
//     .end()
//     .filter("." + $category)
//     .show()
//     .find('img').fadeIn();
// });
// Annual revenue end








// product-slider start here
var swiper = new Swiper(".product-slider", {
  slidesPerView: 3.8,
  spaceBetween: 58,
  speed: 1000,
  loop:true,
  pagination: {
    el: ".swiper-pagination",
    type: "progressbar",
  },
  navigation: {
    nextEl: ".swiper-button-next",
    prevEl: ".swiper-button-prev",
  },
  breakpoints: {
    1400: {
      slidesPerView: 3.8,
      spaceBetween: 30
    },
    768: {
      slidesPerView: 2,
      spaceBetween: 20
    },
    575: {
      slidesPerView: 1,
      spaceBetween: 10,
    }
  },
});


// product-slider end here

//talent_management_slider start here
var swiper = new Swiper(".talent_management_slider", {
  slidesPerView: 1.4,
  spaceBetween: 58,
  grabCursor: true,
  simulateTouch: true,
  speed: 1000,
  pagination: {
    el: ".swiper-pagination",
    type: "progressbar",
  },
  breakpoints: {
    1920: {
      slidesPerView: 1.4,
      spaceBetween: 58
    },
    1199: {
      slidesPerView: 1.1,
      spaceBetween: 30
    },
    1099: {
      slidesPerView: 1,
      spaceBetween: 58
    }
  }
});
//talent_management_slider start here
var swiper = new Swiper(".social-silder", {
  slidesPerView: 3,
  spaceBetween: 20,
  autoplay:true,
  grabCursor: true,
  simulateTouch: true,
  speed: 500,
  pagination: {
    el: ".swiper-pagination",
    type: "progressbar",
  },
  breakpoints: {
    1920: {
      slidesPerView: 3,
      spaceBetween: 20
    },
    1199: {
      slidesPerView: 2,
      spaceBetween: 20
    },
    567: {
      slidesPerView: 1,
      spaceBetween: 20
    }
  }
});


//talent_management_slider end here
//talent-slider start here
var swiper = new Swiper(".talent-slider", {
  slidesPerView: 1,
  spaceBetween: 20,
  autoplay:true,
  grabCursor: true,
  simulateTouch: true,
  speed: 500,
  // pagination: {
  //   el: ".swiper-pagination",
  //   type: "progressbar",
  // },
  // breakpoints: {
  //   1920: {
  //     slidesPerView: 3,
  //     spaceBetween: 20
  //   },
  //   1199: {
  //     slidesPerView: 2,
  //     spaceBetween: 20
  //   },
  //   567: {
  //     slidesPerView: 1,
  //     spaceBetween: 20
  //   }
  // }
});


//talent-slider end here

// manufact-slider start here
var swiper11 = new Swiper(".manufact-slider", {
  slidesPerView: 1.6,
  spaceBetween: 58,
  pagination: {
    el: ".swiper-pagination",
    type: "progressbar",
  },
  grabCursor: true,
  simulateTouch: true,
  speed: 1000,

  breakpoints: {
    1920: {
      slidesPerView: 1.4,
      spaceBetween: 58,
    },
    1199: {
      slidesPerView: 1.1,
      spaceBetween: 30,
    },
    1099: {
      slidesPerView: 1,
      spaceBetween: 25,
    },
  },
});




$(document).ready(function () {
  // Swiper: Slider
  new Swiper('.abt-slide1', {
    loop: false,
    autoHeight: true,
    navigation: {
      nextEl: '.swiper-button-next-2',
      prevEl: '.swiper-button-prev-2',
    },
    slidesPerView: 1,
    paginationClickable: true,
    spaceBetween: 20,
    breakpoints: {
      1920: {
        slidesPerView: 1,
        spaceBetween: 30
      },
      1028: {
        slidesPerView: 1,
        spaceBetween: 30
      },
      480: {
        slidesPerView: 1,
        spaceBetween: 10
      }
    }
  });
});
$(window).on('scroll', function () {
  // console.log(window.scrollY)
  if (window.scrollY > 150) {
    $('header').addClass('header-sticky')
  } else {
    $('header').removeClass('header-sticky')
  }
});

setTimeout(() => {
  var swiper = new Swiper(".mySwiper", {
    loop: true,
    navigation: {
      nextEl: ".swiper-button-next",
      prevEl: ".swiper-button-prev",
    },
    spaceBetween: 30,
  });
}, 1500); // after project_slider fade-in


// upcoming event slide start
$(document).ready(function () {
  const quoteSwiper = new Swiper('.swiper-containe1');
  const imageSwiper = new Swiper('.swiper-containe2', {
    navigation: {
      nextEl: '.swiper-button-next',
      prevEl: '.swiper-button-prev',
    },
  });
  quoteSwiper.controller.control = imageSwiper;
  imageSwiper.controller.control = quoteSwiper;
});
// upcoming event slide end

const swiper1 = new Swiper('.swiper-container', {
  slidesPerView: 1,
  spaceBetween: 35,
  speed: 2000,
  loop: false, // important: don't loop so first stays first
  navigation: {
    nextEl: '.swiper-button-next',
    prevEl: '.swiper-button-prev',
  },
  on: {
    slideChangeTransitionStart: function () {
      document.querySelectorAll('.swiper-slide').forEach((slide, i) => {
        // Remove previous state
        slide.classList.remove('continued-line');

        // Only add class if it's NOT the first slide
        if (i === this.activeIndex && i !== 0) {
          slide.classList.add('continued-line');
        }
      });
    }
  }
});


const swiper3 = new Swiper('.slider_panel .swiper-container', {
  slidesPerView: 1,
  spaceBetween: 0,
  speed: 2000,
  loop: false, // important: don't loop so first stays first
  navigation: {
    nextEl: '.swiper-button-next',
    prevEl: '.swiper-button-prev',
  },
  on: {
    slideChangeTransitionStart: function () {
      document.querySelectorAll('.swiper-slide').forEach((slide, i) => {
        // Remove previous state
        slide.classList.remove('continued-line');

        // Only add class if it's NOT the first slide
        if (i === this.activeIndex && i !== 0) {
          slide.classList.add('continued-line');
        }
      });
    }
  }
});



const swiper2 = new Swiper('.scrollBar_loop .swiper-container', {
  slidesPerView: 3.8,
  spaceBetween: 35,
  speed: 2000,
  loop: false, // important: don't loop so first stays first
  navigation: {
    nextEl: '.swiper-button-next',
    prevEl: '.swiper-button-prev',
  },
  breakpoints: {
    1400: {
      slidesPerView: 3.8,
      spaceBetween: 30
    },
    992: {
      slidesPerView: 2.8,
      spaceBetween: 20
    },
    575: {
      slidesPerView: 1,
      spaceBetween: 10,
    },
  },

})




// document.querySelectorAll('.accordion_hed').forEach(header => {
//   header.addEventListener('click', function () {
//     const content = this.nextElementSibling;
//     const icon = this.querySelector('.toggle-btn');

//     const isOpen = content.style.display === 'block';
//     content.style.display = isOpen ? 'none' : 'block';
//     icon.textContent = isOpen ? '+' : '–';
//   });
// });



$(document).ready(function () {
  // Add .no-dropdown class to li elements without submenu
  $(".accordian_sec li").each(function () {
    if ($(this).children("ul").length === 0) {
      $(this).addClass("no-dropdown");
    }
  });

  // Accordion toggle only on h3 > a
  $(".accordian_sec h3 > a").click(function (e) {
    var $clickedLi = $(this).closest("li");
    var $submenu = $clickedLi.children("ul");

    // If no submenu, allow normal navigation
    if ($submenu.length === 0) {
      return; // Do not preventDefault, allow link to follow
    }

    e.preventDefault(); // Prevent link if submenu exists

    var isActive = $clickedLi.hasClass("active");

    // Close siblings
    $clickedLi
      .siblings("li")
      .removeClass("active")
      .children("ul")
      .slideUp();

    // Toggle current item
    if (!isActive) {
      $clickedLi.addClass("active");
      $submenu.slideDown();
    } else {
      $clickedLi.removeClass("active");
      $submenu.slideUp();
    }
  });

  // Allow nested <a> inside submenu to work normally
  $(".accordian_sec ul ul a").click(function (e) {
    e.stopPropagation();
  });
});



//-====milestone-js-start--//
var swiper = new Swiper(".history_slidernew", {
  loop: false,
  speed: 1000,
  autoplay: {
    delay: 5000,
  },
  navigation: {
    nextEl: ".history-next",
    prevEl: ".history-prev",
  },
  // Responsive breakpoints
  breakpoints: {
    0: {
      slidesPerView: 1,
      spaceBetween: 15,
    },
    500: {
      slidesPerView: 3,
    },
    768: {
      slidesPerView: 4,
    },
    992: {
      slidesPerView: 5,
    },
    1200: {
      slidesPerView: 6,
    },
    1400: {
      slidesPerView: 6,

    },
    1600: {
      slidesPerView: 7,

    },


  }
});
//-====milestone-js-end--//



// invester pge menu start
// Toggle open/close on toggle button
$(".invester-toggle").click(function () {
  $(this).toggleClass("on");
  $("#invester-menu").toggleClass("open");
});

// Also close when clicking on the close class
$(".invester-close").click(function () {
  $(".invester-toggle").removeClass("on");
  $("#invester-menu").removeClass("open");
});
// invester pge menu end



// manufact-slider end here

$(window).on("scroll", function () {
  if ($(this).scrollTop() > 10) {
    $(".eng_header").addClass("top-10");
  } else {
    $(".eng_header").removeClass("top-10");
  }
});

  //-====milestone-js-start--//
var swiper = new Swiper(".history_slidernew", {
    loop: false,
    speed:1000,
    autoplay: {
      delay: 5000,
    },
    navigation: {
      nextEl: ".history-next",
      prevEl: ".history-prev",
    },
     // Responsive breakpoints
     breakpoints: {
        0: {
            slidesPerView:1,
            spaceBetween:15,
          },
        500: {
            slidesPerView:3,
          },
      768: {
          slidesPerView:4,
        },
      992: {
        slidesPerView:5,
      },
      1200: {
        slidesPerView:6,
        },
      1400: {
          slidesPerView:6,

        },
      1600: {
        slidesPerView:7,

      },


    }
  });
  //-====milestone-js-end--//


  // Select input custom 
    
    const selected = document.querySelector(".select-selected");
    const items = document.querySelector(".select-items");

    selected.addEventListener("click", () => {
      items.classList.toggle("show");
    });

    items.querySelectorAll("div").forEach(option => {
      option.addEventListener("click", () => {
        selected.textContent = option.textContent;
        selected.setAttribute("data-value", option.getAttribute("data-value"));
        items.classList.remove("show");
      });
    });

    // close dropdown when clicking outside
    document.addEventListener("click", (e) => {
      if (!e.target.closest(".custom-select")) {
        items.classList.remove("show");
      }
    });
  