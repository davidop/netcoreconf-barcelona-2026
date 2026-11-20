<script setup lang="ts">
import { onBeforeUnmount, onMounted, ref, watch } from 'vue'
import * as THREE from 'three'

const props = withDefaults(defineProps<{status?:string}>(),{status:'planning'})
const host = ref<HTMLElement|null>(null)
const fallback = ref(false)
let renderer:THREE.WebGLRenderer|undefined
let scene:THREE.Scene|undefined
let camera:THREE.PerspectiveCamera|undefined
let figure:THREE.Group|undefined
let wandLight:THREE.PointLight|undefined
let raf = 0
let observer:ResizeObserver|undefined
let start=0
const reduced = typeof matchMedia !== 'undefined' && matchMedia('(prefers-reduced-motion: reduce)').matches

onMounted(()=>{
  if(!host.value || reduced){fallback.value=true;return}
  try {
    const el = host.value
    scene = new THREE.Scene()
    camera = new THREE.PerspectiveCamera(32,1,.1,100)
    camera.position.set(0,1.1,8.6)
    renderer = new THREE.WebGLRenderer({antialias:true,alpha:true})
    renderer.setPixelRatio(Math.min(window.devicePixelRatio,2))
    renderer.outputColorSpace=THREE.SRGBColorSpace
    el.appendChild(renderer.domElement)
    scene.add(new THREE.AmbientLight(0xbad4ff,2.3))
    const key=new THREE.DirectionalLight(0x9ee7ff,3.5);key.position.set(-2,5,6);scene.add(key)
    const fill=new THREE.DirectionalLight(0xbb91ff,2);fill.position.set(3,1,-1);scene.add(fill)
    figure=new THREE.Group();scene.add(figure)
    const mat=(hex:number,metalness=0.05)=>new THREE.MeshStandardMaterial({color:hex,metalness,roughness:.42})
    function sphere(r:number,color:number,x:number,y:number,z:number){
      const mesh=new THREE.Mesh(new THREE.SphereGeometry(r,24,16),mat(color));mesh.position.set(x,y,z);figure!.add(mesh);return mesh
    }
    const cloak=new THREE.Mesh(new THREE.ConeGeometry(1.03,2.05,24,1),mat(0x4356ad,.25))
    cloak.rotation.z=Math.PI;cloak.position.y=-.76;figure.add(cloak)
    sphere(.78,0xf5c5ab,0,.35,.05)
    const hat=new THREE.Mesh(new THREE.ConeGeometry(.88,1.55,26),mat(0x394eaa,.2));hat.position.y=1.65;hat.rotation.z=-.12;figure.add(hat)
    const brim=new THREE.Mesh(new THREE.CylinderGeometry(.95,.95,.12,24),mat(0x5b6bd8));brim.position.y=1.01;figure.add(brim)
    sphere(.095,0x111b41,-.27,.43,.7);sphere(.095,0x111b41,.27,.43,.7)
    sphere(.026,0xffffff,-.24,.46,.79);sphere(.026,0xffffff,.3,.46,.79)
    const staff=new THREE.Mesh(new THREE.CylinderGeometry(.048,.07,2.1,10),mat(0xa7badd,.5))
    staff.rotation.z=-.35;staff.position.set(1.15,-.21,.6);figure.add(staff)
    sphere(.18,0x7ceeff,1.51,.73,.6)
    wandLight=new THREE.PointLight(0x6be3ff,6,3);wandLight.position.set(1.51,.73,.6);figure.add(wandLight)
    for(let i=0;i<9;i++){
      const star=sphere(.035+(i%3)*.018,0x9ce8ff,(i%3-1)*1.65,(i%5-2)*.65, -.5)
      star.material=new THREE.MeshBasicMaterial({color:0xbceaff})
    }
    function resize(){
      if(!host.value||!renderer||!camera)return
      const w=host.value.clientWidth||260,h=host.value.clientHeight||260
      renderer.setSize(w,h,false);camera.aspect=w/h;camera.updateProjectionMatrix()
    }
    observer=new ResizeObserver(resize);observer.observe(el);resize()
    start=performance.now()
    function frame(now:number){
      if(!renderer || !scene || !camera || !figure)return
      const time=(now-start)/1000
      figure.rotation.y=Math.sin(time*.7)*.15
      figure.position.y=Math.sin(time*1.6)*.10
      if(props.status==='blocked') {figure.rotation.z=-.08}
      else if(props.status==='rollback'){figure.rotation.z=Math.sin(time*3)*.07}
      else {figure.rotation.z=0}
      if(wandLight)wandLight.intensity=props.status==='blocked'?1.5:5+Math.sin(time*5)*1.1
      renderer.render(scene,camera)
      raf=requestAnimationFrame(frame)
    }
    raf=requestAnimationFrame(frame)
  }catch{
    fallback.value=true
    renderer?.dispose()
    if(renderer?.domElement.parentElement)renderer.domElement.remove()
  }
})
watch(()=>props.status,()=>{})
onBeforeUnmount(()=>{
  cancelAnimationFrame(raf)
  observer?.disconnect()
  renderer?.dispose()
  if(renderer?.domElement.parentElement)renderer.domElement.remove()
  scene=undefined;camera=undefined;figure=undefined
})
</script>

<template>
  <div ref="host" class="merlin" role="img" :aria-label="'MerlinOps, mago tecnológico. Estado: '+status">
    <svg v-if="fallback" class="merlin-fallback" viewBox="0 0 200 230" aria-hidden="true">
      <path d="M60 122 L35 221 H165 L138 122Z" fill="#4559b6"/>
      <circle cx="100" cy="107" r="51" fill="#f4c6a9"/>
      <path d="M51 73 L92 4 143 79Z" fill="#485cbd"/>
      <ellipse cx="100" cy="77" rx="67" ry="12" fill="#7083dc"/>
      <circle cx="80" cy="108" r="7" fill="#14274c"/><circle cx="119" cy="108" r="7" fill="#14274c"/>
      <path d="M86 129 Q100 142 116 128" fill="none" stroke="#87516b" stroke-width="4" stroke-linecap="round"/>
      <path d="M154 191 L180 64" stroke="#aac9ff" stroke-width="8" stroke-linecap="round"/>
      <circle cx="181" cy="59" r="14" fill="#77ecff"/>
    </svg>
  </div>
</template>

<style scoped>
.merlin{width:100%;max-width:300px;height:255px;position:relative;display:flex;align-items:center;justify-content:center;overflow:hidden}
.merlin canvas{width:100%;height:100%}.merlin-fallback{height:90%;max-width:100%}
@media (prefers-reduced-motion:reduce){.merlin{height:245px}}
</style>
