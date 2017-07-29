<template>
    <Card style="width:320px">
        <div style="text-align:center">
            <h3>注册用户数：{{userTotal}}</h3>
			<h3>机器人数量：{{agentTotal}}</h3>
			<h3>语言本体数量：{{entityTotal}}</h3>
			<h3>对话意图数量：{{intentTotal}}</h3>
			<h3>用户词库：{{userWordTotal}}</h3>
			<h3>企业业务场景：0</h3>
			<h3>行业模板：0</h3>
        </div>
    </Card>
</template>
<script>
	import {HTTP} from '../libs/http-common';
	
    export default {
        data () {
            return {
                userTotal: 0,
				agentTotal: 0,
				entityTotal: 0,
				intentTotal: 0,
				userWordTotal: 0
            }
        },
		created() {
			HTTP.get(`/v1/Dashboard/Statistics`)
				.then(response => {
					this.userTotal = response.data.userTotal;
					this.agentTotal = response.data.agentTotal;
					this.entityTotal = response.data.entityTotal;
					this.intentTotal = response.data.intentTotal;
					this.userWordTotal = response.data.userWordTotal;
				})
				.catch(e => {
				  this.$Message.error(e);
				})
		}
    }
</script>
