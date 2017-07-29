<style scoped>
    .layout{
        border: 1px solid #d7dde4;
        background: #f5f7f9;
        position: relative;
        border-radius: 4px;
        overflow: hidden;
    }
    .layout-breadcrumb{
        padding: 10px 15px 0;
    }
    .layout-content{
        min-height: 200px;
        margin: 15px;
        overflow: hidden;
        background: #fff;
        border-radius: 4px;
    }
    .layout-content-main{
        padding: 10px;
    }
    .layout-copy{
        text-align: center;
        padding: 10px 0 20px;
        color: #9ea7b4;
    }
    .layout-menu-left{
        background: #464c5b;
    }
    .layout-header{
        height: 60px;
        background: #fff;
        box-shadow: 0 1px 1px rgba(0,0,0,.1);
    }
    .layout-logo-left{
        width: 90%;
        height: 30px;
        background: #5b6270;
        border-radius: 3px;
        margin: 15px auto;
    }
    .layout-ceiling-main a{
        color: #9ba7b5;
    }
    .layout-hide-text .layout-text{
        display: none;
    }
    .ivu-col{
        transition: width .2s ease-in-out;
    }
</style>
<template>
    <div class="layout" :class="{'layout-hide-text': spanLeft < 4}">
        <Row type="flex">
            <i-col :span="spanLeft" class="layout-menu-left">
                <Menu active-name="1" theme="dark" width="auto">
                    <div class="layout-logo-left"></div>
                    <Menu-item name="1">
                        <Icon type="ios-keypad" :size="iconSize"></Icon>
                        <span class="layout-text" @click="openMenu('dashboard')">仪表盘</span>
                    </Menu-item>
					<Submenu name="2">
                        <template slot="title">
                            <Icon type="ios-people-outline" :size="iconSize"></Icon>
                            机器人管理
                        </template>
                        <Menu-item name="2-1"><span @click="openMenu('agents')">机器人列表</span></Menu-item>
                    </Submenu>
					<Submenu name="3">
                        <template slot="title">
                            <Icon type="ios-people" :size="iconSize"></Icon>
                            用户管理
                        </template>
                        <Menu-item name="3-1"><span @click="openMenu('users')">注册用户</span></Menu-item>
                    </Submenu>
					<Submenu name="4">
                        <template slot="title">
                            <Icon type="cloud" :size="iconSize"></Icon>
                            YAYA 开放语义平台
                        </template>
                        <Menu-item name="4-1"><span @click="openMenu('users')">词库管理</span></Menu-item>
						<Menu-item name="4-2"><span @click="openMenu('users')">意图管理</span></Menu-item>
						<Menu-item name="4-3"><span @click="openMenu('users')">场景管理</span></Menu-item>
						<Menu-item name="4-4"><span @click="openMenu('apis')">API测试</span></Menu-item>
                    </Submenu>
					<Submenu name="5">
                        <template slot="title">
                            <Icon type="earth" :size="iconSize"></Icon>
                            知识图谱
                        </template>
                        <Menu-item name="5-1"><span @click="openMenu('users')">本体</span></Menu-item>
						<Menu-item name="5-2"><span @click="openMenu('users')">实例</span></Menu-item>
                    </Submenu>
                </Menu>
            </i-col>
            <i-col :span="spanRight">
                <div class="layout-header">
                    <i-button type="text" @click="toggleClick">
                        <Icon type="navicon" size="32"></Icon>
                    </i-button>
                </div>
                <div class="layout-breadcrumb">
                    <Breadcrumb>
                        <Breadcrumb-item href="/">首页</Breadcrumb-item>
                        <Breadcrumb-item>系统仪表盘</Breadcrumb-item>
                    </Breadcrumb>
                </div>
                <div class="layout-content">
                    <div class="layout-content-main"><component :is="content"></component></div>
                </div>
                <div class="layout-copy">
                    © 2012-2017 YayaBot — 深圳爱用科技有限公司
                </div>
            </i-col>
        </Row>
    </div>
</template>
<script>
	import {HTTP} from '../libs/http-common';
	import dashboard from './dashboard.vue'
	import users from './users.vue'
	import agents from './agents.vue'
	
    export default {
        data () {
            return {
                spanLeft: 4,
                spanRight: 20,
				content: 'dashboard'
            }
        },
        computed: {
            iconSize () {
                return this.spanLeft === 4 ? 14 : 24;
            }
        },
        methods: {
            toggleClick () {
                if (this.spanLeft === 4) {
                    this.spanLeft = 2;
                    this.spanRight = 22;
                } else {
                    this.spanLeft = 4;
                    this.spanRight = 20;
                }
            },
			openMenu (component) {
				//this.$router.push(component);
				this.content = component;
				
			}
        },
		components: {
			dashboard: dashboard,
			users: users,
			agents: agents
		}
    }
</script>