const routers = [
    {
        path: '/',
        meta: {
            title: 'YAYABOT 人工智能聊天机器业务管理平台'
        },
        component: (resolve) => require(['./views/index.vue'], resolve)
    },
	{
        path: '/dashboard',
        meta: {
            title: '仪表盘'
        },
        component: (resolve) => require(['./views/dashboard.vue'], resolve)
    }
];
export default routers;